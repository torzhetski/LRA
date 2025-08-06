using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;

namespace LRA.Subscription.Application.Interfaces;

public interface ISubscriptionService
{
    Task HandleWebhooksAsync(StripeWebhookModel webhookModel, CancellationToken cancellationToken);
    Task<bool> HasCustomerActiveSubscription(string customerId, CancellationToken cancellationToken);
}
