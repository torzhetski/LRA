using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Dtos.Account;

namespace LRA.Account.Application.Interfaces;

public interface IAccountService
{
    Task CreateAccountAsync(string email, Guid keycloakId, CancellationToken cancellationToken, CredentialsDetailsEntity credentials = null);
    Task CreateAccountWithTempPasswordAsync(string email, Guid keycloakId, CancellationToken cancellationToken);
    Task<AccountDetailsDto> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<AccountDetailsDto> GetAccountByEmailAsync(string email, CancellationToken cancellationToken);
    Task<AccountDetailsDto> GetAccountByKeycloakIdAsync(Guid keycloakId, CancellationToken cancellationToken);
    PaginatedList<AccountSummaryDto> GetPaginatedAccountsAsync(int pageNumber, int pageSize);
    Task<List<AccountSummaryDto>> GetAllAccountsAsync(CancellationToken cancellationToken);
    Task<List<string>> GetRolesByKeycloakIdAsync(Guid keycloakId, CancellationToken cancellationToken);
    Task UpdateAccountAsync(UpdateAccountDto dto, Guid keycloakId, CancellationToken cancellationToken);
    Task UpdateTwoFaAsync(Guid keycloakId, CancellationToken cancellationToken);
    Task UpdateEmailAsync(string newEmail, Guid keycloakId, CancellationToken cancellationToken);
    Task AddRoleAsync(string roleName, Guid keycloakId, CancellationToken cancellationToken);
    Task RemoveRoleAsync(string roleName, Guid keycloakId, CancellationToken cancellationToken);
    Task SoftDeleteAccountAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> IsTwoFaEnabledAsync(Guid keycloakId, CancellationToken cancellationToken);
    Task VerifyEmailAsync(string email, CancellationToken cancellationToken);
    Task<bool> CanUserLoginAsync(string email, CancellationToken cancellationToken);
    Task MakeTempPasswordUsedAsync(string email, CancellationToken cancellationToken);
}
