using LRA.Account.Domain.Entities;
using LRA.Account.Domain.Models;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Models;
using System.Threading.Tasks;

namespace LRA.Account.Application.Interfaces;

public interface IAccountService
{
    Task CreateAccountAsync(string email, Guid keycloakId, CancellationToken cancellationToken, CredentialsDetailsEntity credentials = null);
    Task CreateAccountWithTempPasswordAsync(string email, Guid keycloakId, CancellationToken cancellationToken);
    Task<AccountDetailsDto> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<AccountDetailsDto> GetAccountByEmailAsync(string email, CancellationToken cancellationToken);
    Task<AccountDetailsDto> GetAccountByKeycloakIdAsync(Guid keycloakId, CancellationToken cancellationToken);
    PaginatedList<AccountSummaryDto> GetPaginatedAccountsAsync(int pageNumber, int pageSize);
    PaginatedList<AccountSummaryDto> FilterAndSearhAccountsAsync(int pageNumber, int pageSize, FilterAndSearchModel model);
    Task<List<AccountSummaryDto>> GetAllAccountsAsync(CancellationToken cancellationToken);
    Task<List<string>> GetRolesByKeycloakIdAsync(Guid keycloakId, CancellationToken cancellationToken);
    Task UpdateAccountAsync(UpdateAccountDto dto, Guid keycloakId, CancellationToken cancellationToken);
    Task UpdateTwoFaAsync(Guid keycloakId, CancellationToken cancellationToken);
    Task UpdateEmailAsync(string newEmail, Guid keycloakId, CancellationToken cancellationToken);
    Task AddRoleAsync(string roleName, Guid keycloakId, CancellationToken cancellationToken);
    Task RemoveRoleAsync(string roleName, Guid keycloakId, CancellationToken cancellationToken);
    Task UpdateSpecifiedAccountAsync(UpdateAccountDto dto, Guid id, Guid adminKeyclaokId, CancellationToken cancellationToken);
    Task UpdateSpecifiedAccountTwoFaAsync(Guid id, Guid adminKeyclaokId, CancellationToken cancellationToken);
    Task AddSpecifiedAccountRoleAsync(string roleName, Guid id, Guid adminKeycloakId, CancellationToken cancellationToken);
    Task RemoveSpecifiedAccountRoleAsync(string roleName, Guid id, Guid adminKeycloakId, CancellationToken cancellationToken);
    Task<DeletedModel> SoftDeleteAccountAsync(Guid id, Guid adminId, CancellationToken cancellationToken);
    Task<DeletedModel> DeleteAccountAsync(Guid id, Guid adminId, CancellationToken cancellationToken);
    Task<bool> IsTwoFaEnabledAsync(Guid keycloakId, CancellationToken cancellationToken);
    Task VerifyEmailAsync(string email, CancellationToken cancellationToken);
    Task<bool> CanUserLoginAsync(string email, CancellationToken cancellationToken);
    Task MakeTempPasswordUsedAsync(string email, CancellationToken cancellationToken);
    Task BlockAccountAsync(Guid id, Guid adminId, DateTime? blockUntilUtc, CancellationToken cancellationToken);
    Task UnblockAccountAsync(Guid id, Guid adminId, CancellationToken cancellationToken);
    Task<TempPasswordModel> CreateTempPassForAccountAsync(Guid id, Guid adminKeycloakId, CancellationToken cancellationToken);
}
