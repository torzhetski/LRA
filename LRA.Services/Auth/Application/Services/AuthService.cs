using LRA.Auth.Application.Interfaces;
using LRA.Auth.Application.Interfaces.Repositories;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Models;

namespace LRA.Auth.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAccountHttpClient _httpClient;
    private readonly IVerificationRepository _verificationRepository;
    private readonly IRabbitMQProducerService _rabbitMQProducerService;

    public AuthService(
        IAccountHttpClient httpClient, 
        IVerificationRepository verificationRepository, 
        IRabbitMQProducerService rabbitMQProducerService)
    {
        _httpClient = httpClient;
        _verificationRepository = verificationRepository;
        _rabbitMQProducerService = rabbitMQProducerService;
    }

    public async Task<bool> IsTwoFaEnabledAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.IsTwoFaEnabledAsync(keycloakId, cancellationToken);
        return response;
    }

    public async Task<string> TwoFaVerificationAsync(string token, CancellationToken cancellationToken)
    {
        var response = await _verificationRepository.ValidateTwoFactorAsync(token, cancellationToken);
        return response;
    }

    public async Task<Guid> SendTwoFactorTokenAsync(string email, string jwtToken, CancellationToken cancellationToken)
    {
        var response = await _verificationRepository.CreateTwoFactorAsync(email, jwtToken, cancellationToken);

        var message = new EmailMessageModel() 
        {
            Content =response.Token,
            RecipientMail = email,
        };
        await _rabbitMQProducerService.SendMessageAsync(message, cancellationToken);

        return response.Id;
    }

    public async Task PasswordResetVerificationAsync(string token, CancellationToken cancellationToken)
    {
        await _verificationRepository.ValidatePasswordResetAsync(token, cancellationToken);
    }

    public async Task<Guid> SendPasswordResetTokenAsync(string email, CancellationToken cancellationToken)
    {
        var response = await _verificationRepository.CreatePasswordResetAsync(email, cancellationToken);

        var message = new EmailMessageModel()
        {
            Content = response.Token,
            RecipientMail = email,
        };
        await _rabbitMQProducerService.SendMessageAsync(message, cancellationToken);

        return response.Id;
    }
}
