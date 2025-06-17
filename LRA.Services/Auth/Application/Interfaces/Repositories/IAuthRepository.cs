using LRA.Shared.Abstructions.Test;

namespace LRA.Auth.Application.Interfaces.Repositories;

public interface IAuthRepository
{
    Guid Create(TestModel model);
    TestModel Get(Guid id);
    List<TestModel> GetAll();
    void Update(TestModel model);
    void Delete(TestModel model);
    Task SaveChangesAsync();
}
