using LRA.Shared.Abstractions.Dtos.Account;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IAccountHttpClient
{
    Task<AccountDto> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<AccountDto> GetByKeycloakIdAsync(string keycloakId, CancellationToken cancellationToken);
    Task<PaginatedList<AccountSummaryDto>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<List<AccountSummaryDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAccountAsync(UpdateAccountDto dto, string keycloakId, CancellationToken cancellationToken);
    Task UpdateTwoFaAsync(string keycloakId, CancellationToken cancellationToken);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<List<string>> GetRolesByKeycloakId(string keycloakId, CancellationToken cancellationToken);
}
