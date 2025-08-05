using LRA.Shared.Abstractions.Dtos.Account;

namespace LRA.Gateways.AdminGateway.Inetrfaces;

public interface IAccountService
{
    Task UpdateAccountAsync(UpdateAccountDto dto, string keycloakId, CancellationToken cancellationToken);
    Task UpdateAccountTwoFa(string keycloakId, CancellationToken cancellationToken);
}
