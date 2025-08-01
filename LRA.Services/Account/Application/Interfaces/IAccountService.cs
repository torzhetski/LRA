using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Dtos.Account;

namespace LRA.Account.Application.Interfaces;

public interface IAccountService
{
    Task<Guid> CreateAccountAsync(AccountEntity model, CancellationToken cancellationToken);
    Task<AccountDto> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<AccountDto> GetAccountByEmailAsync(string email, CancellationToken cancellationToken);
    Task<AccountDto> GetAccountByKeycloakIdAsync(Guid keycloakId, CancellationToken cancellationToken);
    PaginatedList<AccountDto> GetPaginatedAccounts(int pageNumber, int pageSize);
    Task<List<AccountDto>> GetAllAccountsAsync(CancellationToken cancellationToken);
    Task<List<string>> GetRolesByKeycloakIdAsync(Guid keycloakId, CancellationToken cancellationToken);
    Task UpdateAccountAsync(UpdateAccountDto dto, Guid keycloakId, CancellationToken cancellationToken);
    Task UpdateTwoFaAsync(Guid keycloakId, CancellationToken cancellationToken);
    Task UpdateEmailAsync(string newEmail, Guid keycloakId, CancellationToken cancellationToken);
    Task UpdateRolesAsync(List<string> roleNames, Guid keycloakId, CancellationToken cancellationToken);
    Task SoftDeleteAccountAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> IsTwoFaEnabledAsync(Guid keycloakId, CancellationToken cancellationToken);
    Task VerifyEmailAsync(string email, CancellationToken cancellationToken);
}
