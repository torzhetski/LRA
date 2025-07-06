using LRA.Shared.Abstractions.Test;

namespace LRA.Subscription.Application.Interfaces.Repositories;

public interface ISubscriptionRepository
{
    Task<Guid> CreateAsync(TestModel model);
    Task<TestModel> GetAsync(Guid id, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
