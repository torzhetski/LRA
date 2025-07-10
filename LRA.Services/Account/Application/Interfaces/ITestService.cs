using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Test;

namespace LRA.Account.Application.Interfaces;

public interface ITestService
{
    Task<Guid> CreateAccountAsync(AccountEntity model, CancellationToken cancellationToken);
    Task<AccountDto> GetAccountById(Guid id, CancellationToken cancellationToken);
    Task SoftDeleteAccount(Guid id, CancellationToken cancellationToken);
    PaginatedList<AccountDto> GetPaginatedAccounts(int pageNumber, int pageSize);
    Task<List<AccountDto>> GetAllAccountsAsync(CancellationToken cancellationToken);
}
