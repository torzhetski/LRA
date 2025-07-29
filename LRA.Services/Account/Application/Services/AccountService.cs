using LRA.Account.Application.Interfaces;
using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Test;
using LRA.Account.Domain.Mappers;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Utils.Exceptions;

namespace LRA.Account.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmailVerificationRepository _emailVerificationRepository;
    private readonly IRabbitMQProducerService _rabbitMQProducerService;

    public AccountService(
        IAccountRepository accountRepository,
        IEmailVerificationRepository emailVerificationRepository,
        IRabbitMQProducerService rabbitMQProducerService)
    {
        _accountRepository = accountRepository;
        _emailVerificationRepository = emailVerificationRepository;
        _rabbitMQProducerService = rabbitMQProducerService;
    }

    public async Task<Guid> CreateAccountAsync(AccountEntity model, CancellationToken cancellationToken)
    {
        var id = await _accountRepository.CreateAsync(model, cancellationToken);
        var token = await _emailVerificationRepository.CreateVerificationTokenAsync(model.Email, cancellationToken);

        var message = new EmailMessageModel()
        {
            RecipientMail = model.Email,
            Content = $"{token}"
        };

        await _rabbitMQProducerService.SendMessageAsync(message, cancellationToken);
        return id;
    }

    public async Task<AccountDto> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _accountRepository.GetByIdAsync(id, cancellationToken);

        return response;
    }

    public async Task UpdateAccountAsync(AccountDto dto, CancellationToken cancellationToken)
    {
        await _accountRepository.UpdateAsync(dto, cancellationToken);
    }

    public async Task UpdateTwoFaAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        await _accountRepository.UpdateTwoFaAsync(keycloakId, cancellationToken);
    }

    public async Task SoftDeleteAccount(Guid id, CancellationToken cancellationToken)
    {
        await _accountRepository.SoftDeleteAsync(id, cancellationToken);
    }
    
    public PaginatedList<AccountDto> GetPaginatedAccounts(int pageNumber, int pageSize)
    {
        var response = _accountRepository.GetPaginatedData(pageNumber, pageSize);
        return response;
    }

    public async Task<List<AccountDto>> GetAllAccountsAsync(CancellationToken cancellationToken)
    {
        var response = await _accountRepository.GetAllAsync(cancellationToken);
        return response;
    }

    public async Task<bool> VerifyEmailAsync(string token, string email, CancellationToken cancellationToken)
    {
        var result = await _emailVerificationRepository.VerifyTokenAsync(token, email, cancellationToken);
        if (result)
        {
            await _accountRepository.MakeUserVerifiedAsync(email, cancellationToken);
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> IsTwoFaEnabledAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var dto = await _accountRepository.GetByKeycloakIdAsync(keycloakId, cancellationToken);
        if (dto == null)
            throw new NotFoundException(nameof(dto), keycloakId);

        return dto.IsTwoFaEnabled;
    }
}
