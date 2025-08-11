using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Interfaces;
using LRA.Shared.Abstractions.Interfaces.Services;

namespace LRA.Shared.Presentation.Services;

public abstract class AccountServiceBase : IAccountServiceBase, IRoleProvider
{
    protected readonly IAccountHttpClient _httpClient;

    public AccountServiceBase(IAccountHttpClient accountHttpClient)
    {
        _httpClient = accountHttpClient;
    }

    public async Task<AccountDetailsDto> GetAccountAsync(string keycloakId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetByKeycloakIdAsync(keycloakId, cancellationToken);
        return response;
    }

    public async Task UpdateAccountAsync(UpdateAccountDto dto, string keycloakId, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateAccountAsync(dto, keycloakId, cancellationToken);
    }

    public async Task UpdateAccountTwoFa(string keycloakId, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateTwoFaAsync(keycloakId, cancellationToken);
    }

    public async Task<List<string>> GetRolesById(string keycloakId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetRolesByKeycloakId(keycloakId, cancellationToken);
        return response;
    }
}
