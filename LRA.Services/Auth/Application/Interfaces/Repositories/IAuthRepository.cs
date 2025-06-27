using LRA.Shared.Abstractions.Test;

namespace LRA.Auth.Application.Interfaces.Repositories;

public interface IAuthRepository
{
    Guid Create(TestModel model);
    Task<TestModel> GetAsync(Guid id, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
