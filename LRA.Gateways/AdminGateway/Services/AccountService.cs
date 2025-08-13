using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Presentation.Services;

namespace LRA.Gateways.AdminGateway.Services;

public class AccountService : AccountServiceBase, IAdminAccountService
{
    public AccountService(IAccountHttpClient accountHttpClient) : base(accountHttpClient)
    {
    }

    public async Task<List<AccountSummaryDto>> GetAllAccountsAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAllAsync(cancellationToken);
        return response;
    }

    public async Task<PaginatedList<AccountSummaryDto>> GetPaginatedAccountsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetPaginatedAsync(pageNumber, pageSize, cancellationToken);
        return response;
    }

    public async Task<PaginatedList<AccountSummaryDto>> FilterAndSearhAccountsAsync(int pageNumber, int pageSize, FilterAndSearchModel model, CancellationToken cancellationToken)
    {
        var response = await _httpClient.FilterAndSearhAccountsAsync(pageNumber, pageSize, model, cancellationToken);
        return response;
    }

    public async Task<AccountDetailsDto> GetAccountById(Guid id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetByIdAsync(id.ToString(), cancellationToken);
        return response;
    }

    public async Task SoftDeleteAsync(Guid id, string adminId, CancellationToken cancellationToken)
    {
        await _httpClient.SoftDeleteAsync(id, adminId, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, string adminId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync(id, adminId, cancellationToken);
    }

    public async Task BlockAccount(Guid id, string adminKeycloakId, DateTime? blockUntilUtc, CancellationToken cancellationToken)
    {
        await _httpClient.BlockAccount(id, adminKeycloakId, blockUntilUtc, cancellationToken);
    }

    public async Task UnblockAccount(Guid id, string adminKeyclaokId, CancellationToken cancellationToken)
    {
        await _httpClient.UnblockAccount(id, adminKeyclaokId, cancellationToken);
    }

    public async Task UpdateSpecifiedAccountTwoFaAsync(Guid id, string adminKeyclaokId, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateSpecifiedAccountTwoFaAsync(id, adminKeyclaokId, cancellationToken);
    }

    public async Task UpdateSpecifiedAccountAsync(UpdateAccountDto dto, Guid id, string adminKeyclaokId, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateSpecifiedAccountAsync(dto, id, adminKeyclaokId, cancellationToken);
    }
}
