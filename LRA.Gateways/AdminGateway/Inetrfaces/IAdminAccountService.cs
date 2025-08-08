using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces;
using LRA.Shared.Abstractions.Interfaces.Services;

namespace LRA.Gateways.AdminGateway.Inetrfaces;

public interface IAdminAccountService : IAccountServiceBase, IRoleProvider
{
    Task<List<AccountSummaryDto>> GetAllAccountsAsync(CancellationToken cancellationToken);
    Task<PaginatedList<AccountSummaryDto>> GetPaginatedAccountsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
}
