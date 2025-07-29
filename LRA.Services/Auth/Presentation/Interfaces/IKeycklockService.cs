using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Abstractions.Test;

namespace LRA.Auth.Presentation.Interfaces;

public interface IKeycklockService
{
    Task<string> GetKeycloakRealmConfigAsync(CancellationToken cancellationToken);
    Task<string> CreateKeycloakUserAsync(RegistrationModel model, CancellationToken cancellationToken);
    Task<string> LoginKeycloakUserAsync(LoginModel model, CancellationToken cancellationToken);
    Task ChangeUserPasswordAsync (string newPassword, Guid keycloakId, CancellationToken cancellationToken);
}
