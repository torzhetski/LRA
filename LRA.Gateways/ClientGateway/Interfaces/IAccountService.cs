using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Interfaces;

public interface IAccountService
{
    Task<AccountDto> GetAccountAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAccountAsync(UpdateAccountDto dto, string keycloakId, CancellationToken cancellationToken);
    Task UpdateAccountTwoFa(string keycloakId, CancellationToken cancellationToken);
    Task AccountSoftDelete(Guid id, CancellationToken cancellationToken);
}
