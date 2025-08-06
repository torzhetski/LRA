using LRA.Account.Application.Interfaces;
using LRA.Account.Application.Interfaces.Repositories;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Utils.Enums;

namespace LRA.Account.Application.Services;

public class AuthService : IAuthService
{
    private readonly IVerificationRepository _verificationRepository;
    private readonly IRabbitMQProducerService _rabbitMQProducerService;


    public AuthService(
        IVerificationRepository verificationRepository,
        IRabbitMQProducerService rabbitMQProducerService)
    {
        _verificationRepository = verificationRepository;
        _rabbitMQProducerService = rabbitMQProducerService;
    }

    public async Task<bool> VerifyTwoFaTokenAsync(string token, string email, CancellationToken cancellationToken)
    {
        var response = await _verificationRepository.ValidateAsync(token, email, VerificationType.TwoFaVerification ,cancellationToken);
        return response;
    }

    public async Task SendTwoFactorTokenAsync(string email, int tokenLifetime ,CancellationToken cancellationToken)
    {
        var response = await _verificationRepository.CreateAsync(email, VerificationType.TwoFaVerification, tokenLifetime ,cancellationToken);

        var message = new EmailMessageModel() 
        {
            Content = response,
            RecipientMail = email,
        };
        await _rabbitMQProducerService.SendMessageAsync(message, cancellationToken);
    }

    public async Task<bool> VerifyPasswordResetTokenAsync(string token, string email, CancellationToken cancellationToken)
    {
        var response = await _verificationRepository.ValidateAsync(token, email, VerificationType.ResetPassword, cancellationToken);
        return response;
    }

    public async Task SendPasswordResetTokenAsync(string email, int tokenLifetime, CancellationToken cancellationToken)
    {
        var response = await _verificationRepository.CreateAsync(email, VerificationType.ResetPassword, tokenLifetime, cancellationToken);

        var message = new EmailMessageModel()
        {
            Content = response,
            RecipientMail = email,
        };
        await _rabbitMQProducerService.SendMessageAsync(message, cancellationToken);
    }

    public async Task<bool> VerifyEmailAsync(string token, string email, CancellationToken cancellationToken)
    {
        var response = await _verificationRepository.ValidateAsync(token, email, VerificationType.EmailVerification, cancellationToken);
        return response;
    }

    public async Task SendEmailVerificationTokenAsync(string email, int tokenLifetime, CancellationToken cancellationToken)
    {
        var response = await _verificationRepository.CreateAsync(email, VerificationType.EmailVerification, tokenLifetime, cancellationToken);

        var message = new EmailMessageModel()
        {
            Content = response,
            RecipientMail = email,
        };
        await _rabbitMQProducerService.SendMessageAsync(message, cancellationToken);
    }

    public async Task<bool> VerifyEmailChangeAsync(string token, string email, CancellationToken cancellationToken)
    {
        var response = await _verificationRepository.ValidateAsync(token, email, VerificationType.EmailChanging, cancellationToken);
        return response;
    }

    public async Task SendEmailChangingTokenAsync(string email, int tokenLifetime, CancellationToken cancellationToken)
    {
        var response = await _verificationRepository.CreateAsync(email, VerificationType.EmailChanging, tokenLifetime, cancellationToken);

        var message = new EmailMessageModel()
        {
            Content = response,
            RecipientMail = email,
        };
        await _rabbitMQProducerService.SendMessageAsync(message, cancellationToken);
    }
}
