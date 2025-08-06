using LRA.Gateways.Stripe.Inerfaces;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Models;
using Microsoft.Extensions.Primitives;

namespace LRA.Gateways.Stripe.Services;

public class StripeWebhookService : IStripeWebhookService
{
    private readonly IRabbitMQProducerService _producerService;

    public StripeWebhookService(IRabbitMQProducerService producerService)
    {
        _producerService = producerService;
    }

    public async Task HandleWebHookAsync(string body, StringValues headers, CancellationToken cancellationToken)
    {
        var stripeModel = new StripeWebhookModel() 
        { 
            Body = body,
            StripeHeaders = headers.ToString(),
        };
        
        await _producerService.SendMessageAsync(stripeModel, cancellationToken);
    }
}
