using LRA.Shared.Abstractions.Test;

namespace LRA.Subscription.Application.Interfaces.Repositories;

public interface ISubscriptionRepository
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
