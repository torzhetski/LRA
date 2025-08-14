using LRA.Account.Application.Interfaces;
using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Application.Settings;
using LRA.Account.Domain.Entities;
using LRA.Account.Domain.Enums;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LRA.Account.Application.Services;

public class KycService : IKycService
{
    private readonly IKycRepository _kycRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IAzureService _azureService;
    private readonly IRabbitMQProducerService _rabbitMQProducerService;
    private readonly string _queueName;
    private readonly string _queueRoutingKey;

    public KycService(
        IKycRepository kycRepository,
        IAzureService azureService,
        IRabbitMQProducerService rabbitMQProducerService, 
        IAccountRepository accountRepository,
        IOptions<AccountRabbitMqProducerSettings> rabbitMqProducerOptions)
    {
        _kycRepository = kycRepository;
        _azureService = azureService;
        _queueName = rabbitMqProducerOptions.Value.EmailQueue;
        _queueRoutingKey = rabbitMqProducerOptions.Value.EmailQueueRoutingKey;
        _rabbitMQProducerService = rabbitMQProducerService;
        _accountRepository = accountRepository;
    }

    public async Task ApproveKycAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _kycRepository.ApproveKycAsync(userId, cancellationToken);
        var model = await _accountRepository.GetByIdAsync(userId, cancellationToken);

        var email = new EmailMessageModel()
        {
            Content = "Your Kyc approved",
            RecipientMail = model.Email,
        };
        await _rabbitMQProducerService.SendMessageAsync(email, _queueName, _queueRoutingKey, cancellationToken);
    }

    public async Task CreateIfPossibleAsync(Guid userId, IFormFile identityFile, IFormFile certificationFile, IFormFile selfieFile, CancellationToken cancellationToken)
    {
        var identityUrl = await _azureService.UploadPhotoAsync(identityFile.OpenReadStream(), $"{identityFile.FileName}_{DateTime.UtcNow}", cancellationToken);
        var certificationUrl = await _azureService.UploadPhotoAsync(certificationFile.OpenReadStream(), $"{certificationFile.FileName}_{DateTime.UtcNow}", cancellationToken);
        var selfieUrl = await _azureService.UploadPhotoAsync(selfieFile.OpenReadStream(), $"{selfieFile.FileName}_{DateTime.UtcNow}", cancellationToken);

        var enity = new KycEntity()
        {
            AccountId = userId,
            IdentityDocumentUrl = identityUrl.RedirectUrl,
            MedicalCertificationUrl = certificationUrl.RedirectUrl,
            SelfieWithIdentityDocumentUrl = selfieUrl.RedirectUrl,
            Status = KycStatus.InProcess,
        };
        await _kycRepository.CreateIfPossibleAsync(enity, cancellationToken);
    }

    public async Task RejectKycAsync(Guid userId, string rejectMessage, CancellationToken cancellationToken)
    {
        await _kycRepository.RejectKycAsync(userId, rejectMessage, cancellationToken);

        var model = await _accountRepository.GetByIdAsync(userId, cancellationToken);

        var email = new EmailMessageModel()
        {
            Content = rejectMessage,
            RecipientMail = model.Email,
        };
        await _rabbitMQProducerService.SendMessageAsync(email, _queueName, _queueRoutingKey, cancellationToken);
    }
}
