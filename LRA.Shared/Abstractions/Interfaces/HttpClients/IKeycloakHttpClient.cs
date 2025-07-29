using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Abstractions.Test;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IKeycloakHttpClient
{
    Task<string> GetServiceSettingsAsync(CancellationToken cancellationToken);
    Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken);
    Task<string> LoginUserAsync(LoginModel model, CancellationToken cancellationToken);
    Task ChangeUserPasswordAsync(string newPassword, Guid id, CancellationToken cancellationToken);
}
