using LRA.Shared.Abstractions.Test;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IKeycloakHttpClient
{
    Task<string> GetServiceSettingsAsync(CancellationToken cancellationToken);
    Task<string> CreateUserAsync(string email, string password, CancellationToken cancellationToken);
}
