using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Interfaces;

public interface ITestAccountService
{
    Task<TestDto> TestGetFromAccountServiceAsync(Guid id, CancellationToken cancellationToken);
    Task<PaginatedList<TestDto>> TestGetPaginatedFromAccountServiceAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<List<TestDto>> TestGetAllfromAccountServiceAsync(CancellationToken cancellationToken);
    Task<TestMessageModel> TestAccountDatabaseServiceAsync(TestDto model, CancellationToken cancellationToken);
    Task TestBrokerAsync(string message, CancellationToken cancellationToken);
}
