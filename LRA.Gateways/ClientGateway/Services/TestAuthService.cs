using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Services;

public class TestAuthService : ITestAuthService
{
    private readonly IAuthHttpClient _httpClient;

    public TestAuthService(IAuthHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetAuthServiceConfigAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetRealmConfigAsync(cancellationToken);
        return response;
    }

    public async Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken)
    {
        var response = await _httpClient.CreateUserAsync(model, cancellationToken);
        return response;
    }
}
