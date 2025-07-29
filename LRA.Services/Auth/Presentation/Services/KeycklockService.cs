using LRA.Auth.Presentation.Interfaces;
using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Models.Auth;

namespace LRA.Auth.Presentaion.Services;

public class KeycklockService : IKeycklockService
{
    private readonly IKeycloakHttpClient _keycloakHttpClient;
    private readonly IAccountHttpClient _accountHttpClient;

    public KeycklockService(IKeycloakHttpClient keycloakHttpClient, IAccountHttpClient accountHttpClient)
    {
        _keycloakHttpClient = keycloakHttpClient;
        _accountHttpClient = accountHttpClient;
    }

    public async Task<string> GetKeycloakRealmConfigAsync(CancellationToken cancellationToken)
    {
        var response = await _keycloakHttpClient.GetServiceSettingsAsync(cancellationToken);
        return response;
    }

    public async Task<string> CreateKeycloakUserAsync(RegistrationModel model, CancellationToken cancellationToken)
    {
        var response = await _keycloakHttpClient.CreateUserAsync(model, cancellationToken);

        var dto = new BaseAccountDto()
        {
            KeycloakId = Guid.Parse(response),
            Email = model.Email,
        };

        await _accountHttpClient.CreateAccountAsync(dto, cancellationToken);

        return response;
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
}
