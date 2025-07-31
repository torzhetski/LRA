using LRA.Account.Presentation.Interfaces;
using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models.Auth;

namespace LRA.Account.Presentation.Services;

public class KeycloakService : IKeycloakService
{
    private readonly IKeycloakHttpClient _keycloakHttpClient;

    public KeycloakService(IKeycloakHttpClient keycloakHttpClient)
    {
        _keycloakHttpClient = keycloakHttpClient;
    }

    public async Task<string> GetKeycloakRealmConfigAsync(CancellationToken cancellationToken)
    {
        var response = await _keycloakHttpClient.GetServiceSettingsAsync(cancellationToken);
        return response;
    }

    public async Task<string> CreateKeycloakUserAsync(RegistrationModel model, CancellationToken cancellationToken)
    {
        var response = await _keycloakHttpClient.CreateUserAsync(model, cancellationToken);
        return response;
    }

    public async Task EnableKeycloakUserAsync(Guid keycloakUserId, CancellationToken cancellationToken)
    {
        await _keycloakHttpClient.EnableUserAsync(keycloakUserId, cancellationToken);
    }

    public async Task<KeycloakTokenDto> LoginKeycloakUserAsync(LoginModel model, CancellationToken cancellationToken)
    {
        var response = await _keycloakHttpClient.LoginUserAsync(model, cancellationToken);

        return response;
    }

    public async Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        var response = await _keycloakHttpClient.RefreshTokenAsync(refreshTokenModel, cancellationToken);
        return response;
    }

    public async Task LogoutUserSessionAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        await _keycloakHttpClient.LogoutUserSessionAsync(refreshTokenModel, cancellationToken);
    }

    public async Task ChangeUserPasswordAsync(string newPassword, Guid keycloakId, CancellationToken cancellationToken)
    {
        await _keycloakHttpClient.ChangeUserPasswordAsync(newPassword, keycloakId, cancellationToken);
    }

    public async Task ChangeUserEmailAsync(Guid keycloakId, string newEmail, CancellationToken cancellationToken)
    {
        await _keycloakHttpClient.ChangeUserEmailAsync(keycloakId, newEmail, cancellationToken);
    }

    public async Task<KeycloakTokenDto> ExchangeGoogleCodeForTokenAsync(string code, CancellationToken cancellationToken)
    {
        var response = await _keycloakHttpClient.ExchangeGoogleCodeForTokenAsync(code, cancellationToken);
        return response;
    }
}
