using LRA.Shared.Abstractions.Test;

namespace LRA.Account.Application.Interfaces.Repositories;

public interface IAccountRepository
{
    Guid Create(TestModel model);
    Task<TestModel> GetAsync(Guid id, CancellationToken cancellationToken);
    PaginatedList<TestDto> GetPaginatedData(int pageNumber, int pageSize);
    Task<List<TestDto>> GetAllAsync(CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
