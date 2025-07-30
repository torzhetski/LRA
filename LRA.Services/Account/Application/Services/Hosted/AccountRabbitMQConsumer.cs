using LRA.Account.Domain.Entities;
using RabbitMQ.Client.Events;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.RabbitMQProvider.Services;
using LRA.Shared.Abstractions.Settings.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using LRA.Account.Application.Interfaces;

namespace LRA.Account.Application.Services.Hosted;

public class AccountRabbitMQConsumer : RabbitMQConsumerBase
{
    private readonly ILogger<AccountRabbitMQConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public AccountRabbitMQConsumer(
        IOptions<RabbitMQSettings> options,
        ILogger<AccountRabbitMQConsumer> logger,
        IServiceProvider serviceProvider)
        : base(options)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        try
        {
            token.ThrowIfCancellationRequested();
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());

            var dto = JsonSerializer.Deserialize<BaseAccountDto>(content);
            await CreateUserOnDatabaseAsync(dto, token);

            await _channel.BasicAckAsync(ea.DeliveryTag, false, token);
        };

        await _channel.BasicConsumeAsync(
            _options.Value.Consumer.QueueName,
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

    private async Task CreateUserOnDatabaseAsync(BaseAccountDto dto, CancellationToken cancellationToken)
    {
        var model = new AccountEntity()
        {
            KeycloakId = dto.KeycloakId,
            Email = dto.Email,
        };
        using (var scope = _serviceProvider.CreateScope())
        {
            var accountRepository = scope.ServiceProvider.GetService<IAccountService>();
            await accountRepository!.CreateAccountAsync(model, cancellationToken);
        }
    }
}
