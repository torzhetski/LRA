using LRA.Subscription.Domain.Entities;
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
using LRA.Subscription.Application.Interfaces;
using LRA.Shared.Abstractions.Models.Stripe;
using LRA.Subscription.Application.Settings;

namespace LRA.Subscription.Application.Services.Hosted;

public class CustomerRabbitMQConsumer : RabbitMQConsumerBase
{
    private readonly ILogger<CustomerRabbitMQConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<SubscriptionRabbitMqConsumerSettings> _subscriptionOptions;

    public CustomerRabbitMQConsumer(
        IOptions<RabbitMQSettings> options,
        ILogger<CustomerRabbitMQConsumer> logger,
        IServiceProvider serviceProvider,
        IOptions<SubscriptionRabbitMqConsumerSettings> subscriptionOptions)
        : base(options, subscriptionOptions.Value.CustomerUpdateQueue)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _subscriptionOptions = subscriptionOptions;
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

            var dto = JsonSerializer.Deserialize<CustomerCreateModel>(content);
            await StripeCustomerCreate(dto, token);

            await _channel.BasicAckAsync(ea.DeliveryTag, false, token);
        };

        await _channel.BasicConsumeAsync(
            _subscriptionOptions.Value.CustomerUpdateQueue,
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

    private async Task StripeCustomerCreate(CustomerCreateModel model, CancellationToken token)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var _customerService = scope.ServiceProvider.GetRequiredService<ICustomerProviderService>();
            
            await _customerService.CreateStripeCustomer(model.KeycloakId, model.Email, token);
        }
    }
}
