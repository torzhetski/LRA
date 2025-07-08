using LRA.Shared.Abstractions.Test;

namespace LRA.Account.Application.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<Guid> CreateAsync(TestModel model);
    Task<TestModel> GetAsync(Guid id, CancellationToken cancellationToken);
    void SoftDelete (Guid id);
    Task SoftDeleteAsync (Guid id, CancellationToken cancellationToken);
    void Delete(Guid id);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
