using LRA.Shared.Abstractions.Test;

namespace LRA.Account.Application.Interfaces.Repositories;

public interface IAccountRepository
{
    Guid Create(TestModel model);
    TestModel GetAsync(Guid id);
    List<TestModel> GetAllAsync();
    void Update(TestModel model);
    void Delete(TestModel model);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
