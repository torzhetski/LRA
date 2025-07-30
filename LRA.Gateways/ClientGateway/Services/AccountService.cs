using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Services;

public class AccountService : IAccountService
{
    private readonly IAccountHttpClient _httpClient;

    public AccountService(IAccountHttpClient accountHttpClient)
    {
        _httpClient = accountHttpClient;
    }

    public async Task<AccountDto> GetFromAccountServiceAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetByIdAsync(id, cancellationToken);
        return response;
    }

    public async Task<PaginatedList<AccountDto>> GetPaginatedFromAccountServiceAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetPaginatedAsync(pageNumber, pageSize, cancellationToken);
        return response;
    }

    public async Task<List<AccountDto>> GetAllFromAccountServiceAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAllAsync(cancellationToken);
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
}
