using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Abstractions.Dtos;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IKeycloakHttpClient
{
    Task<string> GetServiceSettingsAsync(CancellationToken cancellationToken);
    Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken);
    Task EnableUserAsync(Guid keycloakUserId, CancellationToken cancellationToken);
    Task<KeycloakTokenDto> LoginUserAsync(LoginModel model, CancellationToken cancellationToken);
    Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken);
    Task LogoutUserSessionAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken);
    Task ChangeUserPasswordAsync(string newPassword, Guid keycloakId, CancellationToken cancellationToken);
    Task ChangeUserEmailAsync(Guid keycloakId, string newEmail, CancellationToken cancellationToken);
}
