using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;

namespace LRA.Gateways.Client.Services;

public class AuthService : IAuthService
{
    private readonly IAuthHttpClient _httpClient;

    public AuthService(IAuthHttpClient httpClient)
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

    public async Task<string> LoginUserAsync(LoginModel model, CancellationToken cancellationToken)
    {
        var response = await _httpClient.LoginUserAsync(model, cancellationToken);
        return response;
    }
}
