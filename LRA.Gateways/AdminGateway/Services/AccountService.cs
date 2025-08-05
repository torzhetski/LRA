using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;

namespace LRA.Gateways.AdminGateway.Services;

public class AccountService : IAccountService, IRoleProvider
{
    private readonly IAccountHttpClient _httpClient;

    public AccountService(IAccountHttpClient accountHttpClient)
    {
        _httpClient = accountHttpClient;
    }

    public Task UpdateAccountAsync(UpdateAccountDto dto, string keycloakId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAccountTwoFa(string keycloakId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetRolesById(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
