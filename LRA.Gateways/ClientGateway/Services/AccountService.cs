using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;

namespace LRA.Gateways.Client.Services;

public class AccountService : IAccountService, IRoleProvider
{
    private readonly IAccountHttpClient _httpClient;

    public AccountService(IAccountHttpClient accountHttpClient)
    {
        _httpClient = accountHttpClient;
    }

    public async Task<AccountDto> GetAccountAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetByIdAsync(id, cancellationToken);
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

    public async Task AccountSoftDelete(Guid id, CancellationToken cancellationToken)
    {
        await _httpClient.SoftDeleteAsync(id, cancellationToken);
    }

    public async Task<List<string>> GetRolesById(string keycloakId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetRolesByKeycloakId(keycloakId, cancellationToken);
        return response;
    }
}
