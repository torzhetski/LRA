using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Interfaces;

public interface IAccountService
{
    Task<AccountDto> GetFromAccountServiceAsync(Guid id, CancellationToken cancellationToken);
    Task<PaginatedList<AccountDto>> GetPaginatedFromAccountServiceAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<List<AccountDto>> GetAllFromAccountServiceAsync(CancellationToken cancellationToken);
    Task AccountSoftDelete(Guid id, CancellationToken cancellationToken);
    Task UpdateAccountTwoFa(string keycloakId, CancellationToken cancellationToken);
}
