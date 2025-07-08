using LRA.Shared.Abstractions.Test;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IAccountHttpClient
{
    Task<TestDto> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<PaginatedList<TestDto>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<List<TestDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<TestMessageModel> PostAsync(TestDto model, CancellationToken cancellationToken);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
    Task PostMessageAsync(string message, CancellationToken cancellationToken);
}
