using LRA.Auth.Presentation.Interfaces;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Models;

namespace LRA.Auth.Presentaion.Services;

public class KeycklockService : IKeycklockService
{
    private readonly IKeycloakHttpClient _keycloakHttpClient;
    private readonly IRabbitMQProducerService _rabbitMQProducerService;

    public KeycklockService(IKeycloakHttpClient keycloakHttpClient, IRabbitMQProducerService rabbitMQProducerService)
    {
        _keycloakHttpClient = keycloakHttpClient;
        _rabbitMQProducerService = rabbitMQProducerService;
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

        await _rabbitMQProducerService.SendMessageAsync(dto, cancellationToken);

        return response;
    }

    public async Task<string> LoginKeycloakUserAsync(LoginModel model, CancellationToken cancellationToken)
    {
        var response = await _keycloakHttpClient.LoginUserAsync(model, cancellationToken);

        return response;
    }
}
