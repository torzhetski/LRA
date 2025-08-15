using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Models;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IAccountHttpClient
{
    Task<PaginatedList<AccountSummaryDto>> FilterAndSearhAccountsAsync(int pageNumber, int pageSize, FilterAndSearchModel model, CancellationToken cancellationToken);
    Task<AccountDetailsDto> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<AccountDetailsDto> GetByKeycloakIdAsync(string keycloakId, CancellationToken cancellationToken);
    Task<PaginatedList<AccountSummaryDto>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<List<AccountSummaryDto>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAccountAsync(UpdateAccountDto dto, string keycloakId, CancellationToken cancellationToken);
    Task UpdateTwoFaAsync(string keycloakId, CancellationToken cancellationToken);
    Task SoftDeleteAsync(Guid id, string keycloakId, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, string keycloakId, CancellationToken cancellationToken);
    Task<List<string>> GetRolesByKeycloakId(string keycloakId, CancellationToken cancellationToken);
    Task BlockAccount(Guid id, string adminKeycloakId, DateTime? blockUntilUtc, CancellationToken cancellationToken);
    Task UnblockAccount(Guid id, string adminKeyclaokId, CancellationToken cancellationToken);
    Task UpdateSpecifiedAccountTwoFaAsync(Guid id, string adminKeycloakId, CancellationToken cancellationToken);
    Task UpdateSpecifiedAccountAsync(UpdateAccountDto dto, Guid id, string adminKeycloakId, CancellationToken cancellationToken);
}
