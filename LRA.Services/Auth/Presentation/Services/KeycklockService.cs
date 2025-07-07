using LRA.Auth.Presentation.Interfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;

namespace LRA.Auth.Presentaion.Services;

public class KeycklockService : IKeycklockService
{
    private readonly IKeycloakHttpClient _keycloakHttpClient;

    public KeycklockService(IKeycloakHttpClient keycloakHttpClient)
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
        //TODO: response will be id of user here we need to produce it to brocker
        return response;
    }
}
