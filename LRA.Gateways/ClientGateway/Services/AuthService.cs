using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models.Auth;

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

    public async Task<VerificationDto> LoginUserAsync(LoginModel model, CancellationToken cancellationToken)
    {
        var response = await _httpClient.LoginUserAsync(model, cancellationToken);
        return response;
    }

    public async Task<VerificationDto> TwoFactorVerificateAsync(string token, CancellationToken cancellationToken)
    {
        var response = await _httpClient.TwoFactorVerificateAsync(token, cancellationToken);
        return response;
    }

    public async Task ChangeUserPasswordAsync(string newPassword, string email, CancellationToken cancellationToken)
    {
        await _httpClient.ChangeUserPasswordAsync(newPassword, email, cancellationToken);
    }
}
