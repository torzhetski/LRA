using Microsoft.Extensions.Primitives;

namespace LRA.Gateways.Stripe.Inerfaces;

public interface IStripeWebhookService
{
    Task HandleWebHookAsync(string body, StringValues headers, CancellationToken cancellationToken);
}
