using LRA.Gateways.Stripe.Inerfaces;
using LRA.Gateways.Stripe.Settings;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace LRA.Gateways.Stripe.Services;

public class StripeWebhookService : IStripeWebhookService
{
    private readonly IRabbitMQProducerService _producerService;
    private readonly string _queueName;
    public readonly string _queueRoutingKey;

    public StripeWebhookService(IRabbitMQProducerService producerService, IOptions<StripeRabbitMqProducerSettings> stripeOptions)
    {
        _producerService = producerService;
        _queueName = stripeOptions.Value.WebhookQueue;
        _queueRoutingKey = stripeOptions.Value.WebhookQueueRoutingKey;
    }

    public async Task HandleWebHookAsync(string body, StringValues headers, CancellationToken cancellationToken)
    {
        var stripeModel = new StripeWebhookModel() 
        { 
            Body = body,
            StripeHeaders = headers.ToString(),
        };
        
        await _producerService.SendMessageAsync(stripeModel, _queueName, _queueRoutingKey, cancellationToken);
    }
}
