using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Services;

public class TestAuthService : ITestAuthService
{
    private readonly IAuthHttpClient _httpClient;

    public TestAuthService(IAuthHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TestMessageModel> TestAuthServiceAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(id, cancellationToken);
        return response;
    }
}
