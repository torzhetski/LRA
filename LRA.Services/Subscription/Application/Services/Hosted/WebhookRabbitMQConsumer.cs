using RabbitMQ.Client.Events;
using LRA.Shared.Abstractions.RabbitMQProvider.Services;
using LRA.Shared.Abstractions.Settings.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using LRA.Subscription.Application.Interfaces;
using LRA.Shared.Abstractions.Models;
using LRA.Subscription.Domain.Settings;

namespace LRA.Subscription.Application.Services.Hosted;

public class WebhookRabbitMQConsumer : RabbitMQConsumerBase
{
    private readonly ILogger<WebhookRabbitMQConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<SubscriptionRabbitMqConsumerSettings> _subscriptionOptions;

    public WebhookRabbitMQConsumer(
        IOptions<RabbitMQSettings> options,
        ILogger<WebhookRabbitMQConsumer> logger,
        IServiceProvider serviceProvider,
        IOptions<SubscriptionRabbitMqConsumerSettings> subscriptionOptions)
        : base(options, subscriptionOptions.Value.WebhookQueue)
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

            var dto = JsonSerializer.Deserialize<StripeWebhookModel>(content);
            await ProcessWebhookAsync(dto, token);

            await _channel.BasicAckAsync(ea.DeliveryTag, false, token);
        };

        await _channel.BasicConsumeAsync(
            _subscriptionOptions.Value.WebhookQueue,
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

    private async Task ProcessWebhookAsync(StripeWebhookModel dto, CancellationToken token)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var subscriptionService = scope.ServiceProvider.GetService<ISubscriptionService>();
            await subscriptionService!.HandleWebhooksAsync(dto, token);
        }
    }
}
