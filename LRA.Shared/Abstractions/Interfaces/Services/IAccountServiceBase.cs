using LRA.Shared.Abstractions.Dtos.Account;

namespace LRA.Shared.Abstractions.Interfaces.Services;

public interface IAccountServiceBase
{
    Task<AccountDto> GetAccountAsync(string keycloakId, CancellationToken cancellationToken);
    Task UpdateAccountAsync(UpdateAccountDto dto, string keycloakId, CancellationToken cancellationToken);
    Task UpdateAccountTwoFa(string keycloakId, CancellationToken cancellationToken);
}
