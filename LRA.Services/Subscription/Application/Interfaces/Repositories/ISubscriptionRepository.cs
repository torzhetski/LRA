using LRA.Shared.Abstractions.Test;
using LRA.Subscription.Domain.Entities;

namespace LRA.Subscription.Application.Interfaces.Repositories;

public interface ISubscriptionRepository
{
    Task AddSubsriptionAsync(SubscriptionEntity entity, CancellationToken token);
    Task UpdateSubsriptionAsync(SubscriptionEntity entity, CancellationToken token);
    Task<bool> HasCustomerActiveSubscription(string customerId, CancellationToken cancellationToken)
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
