using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;

namespace LRA.Auth.Presentation.Interfaces;

public interface IKeycklockService
{
    Task<string> GetKeycloakRealmConfigAsync(CancellationToken cancellationToken);
    Task<string> CreateKeycloakUserAsync(RegistrationModel model, CancellationToken cancellationToken);
    Task<KeycloakTokenDto> LoginKeycloakUserAsync(LoginModel model, CancellationToken cancellationToken);
    Task<KeycloakTokenDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
