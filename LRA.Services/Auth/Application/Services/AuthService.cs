using LRA.Auth.Application.Interfaces;
using LRA.Auth.Application.Interfaces.Repositories;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Models;

namespace LRA.Auth.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAccountHttpClient _httpClient;
    private readonly ITwoFaRepository _authRepository;
    private readonly IRabbitMQProducerService _rabbitMQProducerService;

    public AuthService(
        IAccountHttpClient httpClient, 
        ITwoFaRepository authRepository, 
        IRabbitMQProducerService rabbitMQProducerService)
    {
        _httpClient = httpClient;
        _authRepository = authRepository;
        _rabbitMQProducerService = rabbitMQProducerService;
    }

    public async Task<bool> IsTwoFaEnabledAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.IsTwoFaEnabledAsync(keycloakId, cancellationToken);
        return response;
    }

    public async Task<string> TwoFaVerification(string token, Guid id, CancellationToken cancellationToken)
    {
        var response = await _authRepository.ValidateAsync(token, id, cancellationToken);
        return response;
    }

    public async Task<Guid> SendTwoFactorToken(string email, string jwtToken, CancellationToken cancellationToken)
    {
        var response = await _authRepository.CreateAsync(email, jwtToken, cancellationToken);

        var message = new EmailMessageModel() 
        {
            Content =response.Token,
            RecipientMail = email,
        };
        await _rabbitMQProducerService.SendMessageAsync(message, cancellationToken);

        return response.Id;
    }
}
