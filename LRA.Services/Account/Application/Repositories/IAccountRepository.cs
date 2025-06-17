using LRA.Shared.Abstructions.Test;

namespace LRA.Account.Application.Repositories;

public interface IAccountRepository
{
    Guid Create(TestModel model);
    TestModel Get(Guid id);
    List<TestModel> GetAll();
    void Update(TestModel model);
    void Delete(TestModel model);
    Task SaveChangesAsync();
}
