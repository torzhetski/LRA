using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Dtos;
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

    public async Task<VerificationDto> TwoFactorVerificateAsync(TwoFactorLoginModel model, CancellationToken cancellationToken)
    {
        var response = await _httpClient.TwoFactorVerificateAsync(model, cancellationToken);
        return response;
    }

    public async Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        var response = await _httpClient.RefreshTokenAsync(refreshTokenModel, cancellationToken);
        return response;
    }

    public async Task LogoutUserSessionAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        await _httpClient.LogoutUserSessionAsync(refreshTokenModel, cancellationToken);
    }

    public async Task ChangeUserPasswordAsync(string newPassword, Guid keycloakId, CancellationToken cancellationToken)
    {
        await _httpClient.ChangeUserPasswordAsync(newPassword, keycloakId, cancellationToken);
    }
}
