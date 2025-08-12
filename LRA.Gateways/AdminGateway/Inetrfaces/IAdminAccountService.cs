using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces;
using LRA.Shared.Abstractions.Interfaces.Services;
using LRA.Shared.Abstractions.Models;

namespace LRA.Gateways.AdminGateway.Inetrfaces;

public interface IAdminAccountService : IAccountServiceBase, IRoleProvider
{
    Task<List<AccountSummaryDto>> GetAllAccountsAsync(CancellationToken cancellationToken);
    Task<PaginatedList<AccountSummaryDto>> GetPaginatedAccountsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<PaginatedList<AccountSummaryDto>> FilterAndSearhAccountsAsync(int pageNumber, int pageSize, FilterAndSearchModel model, CancellationToken cancellationToken);
    Task<AccountDetailsDto> GetAccountById(Guid id, CancellationToken cancellationToken);
    Task SoftDeleteAsync(Guid id, string adminId, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, string adminId, CancellationToken cancellationToken);
}
