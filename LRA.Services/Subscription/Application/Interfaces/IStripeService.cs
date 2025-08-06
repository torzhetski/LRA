using LRA.Shared.Abstractions.Models;

namespace LRA.Subscription.Application.Interfaces;

public interface IStripeService
{
    Task<RedirectUrlModel> CreateCheckoutSessionAsync(string customerId, string priceId, CancellationToken cancellationToken);
    Task<RedirectUrlModel> CreateBillingPortalSessionAsync(string customerId, CancellationToken cancellationToken);
    Task<string> CreateStripeCustomerAsync(string email, CancellationToken cancellationToken);
}
