using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Models;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IAccountHttpClient
{
    Task<AccountDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PaginatedList<AccountDto>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<List<AccountDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAccountAsync(AccountDto dto, CancellationToken cancellationToken);
    Task UpdateTwoFaAsync(string keycloakId, CancellationToken cancellationToken);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
}
