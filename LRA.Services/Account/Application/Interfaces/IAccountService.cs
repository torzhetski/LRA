using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Dtos.Account;

namespace LRA.Account.Application.Interfaces;

public interface IAccountService
{
    Task<Guid> CreateAccountAsync(AccountEntity model, CancellationToken cancellationToken);
    Task<AccountDto> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAccountAsync(AccountDto dto, CancellationToken cancellationToken);
    Task UpdateTwoFaAsync(Guid keycloakId, CancellationToken cancellationToken);
    Task SoftDeleteAccount(Guid id, CancellationToken cancellationToken);
    PaginatedList<AccountDto> GetPaginatedAccounts(int pageNumber, int pageSize);
    Task<List<AccountDto>> GetAllAccountsAsync(CancellationToken cancellationToken);
    Task<bool> VerifyEmailAsync(string token, string email, CancellationToken cancellationToken);
    Task<bool> IsTwoFaEnabledAsync(Guid keycloakId, CancellationToken cancellationToken);
}
