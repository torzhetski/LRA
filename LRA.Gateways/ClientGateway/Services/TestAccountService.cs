using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Services;

public class TestAccountService : ITestAccountService
{
    private readonly IAccountHttpClient _httpClient;

    public TestAccountService (IAccountHttpClient accountHttpClient)
    {
        _httpClient = accountHttpClient;
    }

    public async Task<TestDto> TestGetFromAccountServiceAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(id, cancellationToken);
        return response;
    }

    public async Task<TestMessageModel> TestAccountDatabaseServiceAsync(TestDto model, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync(model, cancellationToken);
        return response;
    }


    public async Task TestAccountSoftDelete(Guid id, CancellationToken cancellationToken)
    {
        await _httpClient.SoftDeleteAsync(id, cancellationToken);
    }
    
    public async Task<PaginatedList<TestDto>> TestGetPaginatedFromAccountServiceAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetPaginatedAsync(pageNumber, pageSize, cancellationToken);
        return response;
    }

    public async Task<List<TestDto>> TestGetAllfromAccountServiceAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAllAsync(cancellationToken);
        return response;
    }

    public async Task TestBrokerAsync(string message, CancellationToken cancellationToken)
    {
        await _httpClient.PostMessageAsync(message, cancellationToken);
    }
}
