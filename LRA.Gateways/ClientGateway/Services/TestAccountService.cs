using LRA.Gateways.Client.Interfaces;
using LRA.Gateways.Client.Options;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstructions.Test;
using Microsoft.Extensions.Options;
using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Services;

public class TestAccountService : ITestAccountService
{
    private readonly IAccountHttpClient _httpClient;

    public TestAccountService (IAccountHttpClient accountHttpClient)
    {
        _httpClient = accountHttpClient;
    }

    public async Task<TestMessageModel> TestAccountServiceAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(id, cancellationToken);
        return response;
    }

    public async Task<TestMessageModel> TestAccountDatabaseServiceAsync(TestModel model, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync(model, cancellationToken);
        return response;
    }
}
