using LRA.Shared.Abstractions.Test;

namespace LRA.Subscription.Application.Interfaces.Repositories;

public interface ISubscriptionRepository
{
    Guid Create(TestModel model);
    Task<TestModel> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<List<TestModel>> GetAllAsync(CancellationToken cancellationToken);
    void Update(TestModel model);
    void Delete(TestModel model);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
