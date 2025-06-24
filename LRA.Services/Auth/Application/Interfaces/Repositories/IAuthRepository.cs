using LRA.Shared.Abstractions.Test;

namespace LRA.Auth.Application.Interfaces.Repositories;

public interface IAuthRepository
{
    Guid Create(TestModel model);
    TestModel GetAsync(Guid id);
    List<TestModel> GetAllAsync();
    void Update(TestModel model);
    void Delete(TestModel model);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
