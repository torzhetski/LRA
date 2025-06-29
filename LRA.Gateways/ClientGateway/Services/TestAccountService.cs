using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Test;
using Microsoft.Extensions.Options;

namespace LRA.Gateways.Client.Services;

public class TestAccountService : ITestAccountService
{
    private readonly IAccountHttpClient _httpClient;

    public TestAccountService (IAccountHttpClient accountHttpClient)
    {
        _httpClient = accountHttpClient;
    }

    public async Task<TestDto> TestAccountServiceAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(id, cancellationToken);
        return response.FromModelToDto();
    }

    public async Task<TestMessageModel> TestAccountDatabaseServiceAsync(TestDto model, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync(model, cancellationToken);
        return response;
    }

    public async Task TestBrokerAsync(string message, CancellationToken cancellationToken)
    {
        await _httpClient.MessageAsync(message, cancellationToken);
    }
}
