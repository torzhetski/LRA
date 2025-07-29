using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Models.Auth;

namespace LRA.Auth.Presentation.Interfaces;

public interface IKeycklockService
{
    Task<string> GetKeycloakRealmConfigAsync(CancellationToken cancellationToken);
    Task<string> CreateKeycloakUserAsync(RegistrationModel model, CancellationToken cancellationToken);
    Task<KeycloakTokenDto> LoginKeycloakUserAsync(LoginModel model, CancellationToken cancellationToken);
    Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken);
    Task LogoutUserSessionAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken);
}
