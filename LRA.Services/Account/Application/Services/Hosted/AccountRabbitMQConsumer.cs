using RabbitMQ.Client.Events;
using LRA.Shared.Abstractions.RabbitMQProvider.Services;
using LRA.Shared.Abstractions.Settings.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using LRA.Account.Application.Interfaces;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Utils.Enums;
using LRA.Account.Application.Settings;

namespace LRA.Account.Application.Services.Hosted;

public class AccountRabbitMQConsumer : RabbitMQConsumerBase
{
    private readonly ILogger<AccountRabbitMQConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<AccountRabbitMqConsumerSettings> _accountOptions;

    public AccountRabbitMQConsumer(
        IOptions<RabbitMQSettings> options,
        ILogger<AccountRabbitMQConsumer> logger,
        IServiceProvider serviceProvider,
        IOptions<AccountRabbitMqConsumerSettings> accountOptions)
        : base(options, accountOptions.Value.RoleUpdateQueue)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _accountOptions = accountOptions;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        try
        {
            token.ThrowIfCancellationRequested();
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(
            ex,
            "The background consumer task was cancelled: {ErrorDescription}",
            ex.Message
            );
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());

            var role = JsonSerializer.Deserialize<UpdateRoleModel>(content);
            await UpdateUserRolesAsync(role, token);

            await _channel.BasicAckAsync(ea.DeliveryTag, false, token);
        };

        await _channel.BasicConsumeAsync(
            _accountOptions.Value.RoleUpdateQueue,
            false,
            consumer,
            token);
    }

    public override void Dispose()
    {
        _connection?.Dispose();
        _channel?.Dispose();
        base.Dispose();
    }

    private async Task UpdateUserRolesAsync(UpdateRoleModel role, CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var accountService = scope.ServiceProvider.GetService<IAccountService>();

            switch (role.UpdateType)
            {
                case RoleUpdateType.Add:
                    await accountService.AddRoleAsync(role.RoleName, role.KeycloakId, cancellationToken);
                    break;

                case RoleUpdateType.Remove:
                    await accountService.RemoveRoleAsync(role.RoleName, role.KeycloakId, cancellationToken);
                    break;
            }
        }
    }
}
