using LRA.Shared.Abstructions.Test;

namespace LRA.Subscription.Application.Interfaces.Repositories;

public interface ISubscriptionRepository
{
    Guid Create(TestModel model);
    TestModel Get(Guid id);
    List<TestModel> GetAll();
    void Update(TestModel model);
    void Delete(TestModel model);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
