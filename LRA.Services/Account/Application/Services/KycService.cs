using LRA.Account.Application.Interfaces;
using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Application.Settings;
using LRA.Account.Domain.Entities;
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
        
    }

    public Task CreateIfPossibleAsync(IFormFile identityFile, IFormFile certificationFile, IFormFile selfieFile, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task RejectKycAsync(Guid userId, string rejectMessage, CancellationToken cancellationToken)
    {
        await _kycRepository.RejectKycAsync(userId, rejectMessage, cancellationToken);
    }
}
