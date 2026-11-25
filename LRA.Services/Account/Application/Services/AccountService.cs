using LRA.Account.Application.Interfaces;
using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Application.Settings;
using LRA.Account.Domain.Entities;
using LRA.Account.Domain.Models;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Models.Stripe;
using LRA.Shared.Utils;
using LRA.Shared.Utils.Enums;
using LRA.Shared.Utils.Exceptions;
using Microsoft.Extensions.Options;

namespace LRA.Account.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IRabbitMQProducerService _rabbitMQProducerService;
    private readonly string _queueName;
    private readonly string _queueRoutingKey;

    public AccountService(
        IAccountRepository accountRepository,
        IRabbitMQProducerService rabbitMQProducerService,
        IOptions<AccountRabbitMqProducerSettings> rabbitMqProducerOptions)
    {
        _accountRepository = accountRepository;
        _rabbitMQProducerService = rabbitMQProducerService;
        _queueName = rabbitMqProducerOptions.Value.SubscriptionQueue;
        _queueRoutingKey = rabbitMqProducerOptions.Value.SubscriptionQueueRoutingKey;
    }

    public async Task CreateAccountAsync(string email, Guid keycloakId, CancellationToken cancellationToken, CredentialsDetailsEntity credentials)
    {
        if (!await _accountRepository.IsAccountExists(keycloakId, cancellationToken))
        {
            var model = new AccountEntity()
            {
                KeycloakId = keycloakId,
                Email = email,
                IsEmailVerified = true,
                CredentialsDetails = credentials,
            };
            var id = await _accountRepository.CreateAsync(model, cancellationToken);

            var customerModel = new CustomerChangeModel()
            {
                KeycloakId = keycloakId,
                Email = email,
                Type = CustomerChangeType.Create,
            };
            await _rabbitMQProducerService.SendMessageAsync(customerModel, _queueName, _queueRoutingKey, cancellationToken);
        }
    }

    public async Task CreateAccountWithTempPasswordAsync(string email, Guid keycloakId, CancellationToken cancellationToken)
    {
        var credentials = new CredentialsDetailsEntity()
        {
            HasTempPassword = true,
            IsPasswordUsed = false,
            ExpiresAt = DateTime.UtcNow.AddDays(1),
        };
        await CreateAccountAsync(email, keycloakId, cancellationToken, credentials);
    }

    public async Task<AccountDetailsDto> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _accountRepository.GetByIdAsync(id, cancellationToken);

        return response;
    }

    public async Task<AccountDetailsDto> GetAccountByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var response = await _accountRepository.GetByEmailAsync(email, cancellationToken);
        return response;
    }

    public async Task<AccountDetailsDto> GetAccountByKeycloakIdAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var response = await _accountRepository.GetByKeycloakIdAsync(keycloakId, cancellationToken);
        return response;
    }

    public PaginatedList<AccountSummaryDto> FilterAndSearhAccountsAsync(int pageNumber, int pageSize, FilterAndSearchModel model)
    {
        var response = _accountRepository.FilterAndSearhAccountsAsync(pageNumber, pageSize, model);
        return response;
    }

    public PaginatedList<AccountSummaryDto> GetPaginatedAccountsAsync(int pageNumber, int pageSize)
    {
        var response = _accountRepository.GetPaginatedAccountsAsync(pageNumber, pageSize);
        return response;
    }

    public async Task<List<AccountSummaryDto>> GetAllAccountsAsync(CancellationToken cancellationToken)
    {
        var response = await _accountRepository.GetAllAccountsAsync(cancellationToken);
        return response;
    }

    public async Task<List<string>> GetRolesByKeycloakIdAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var response = await _accountRepository.GetRolesByKeycloakIdAsync(keycloakId, cancellationToken);
        return response;
    }

    public async Task UpdateAccountAsync(UpdateAccountDto dto, Guid keycloakId, CancellationToken cancellationToken)
    {
        await _accountRepository.UpdateAsync(dto, keycloakId, cancellationToken);
    }

    public async Task UpdateTwoFaAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        await _accountRepository.UpdateTwoFaAsync(keycloakId, cancellationToken);
    }

    public async Task UpdateEmailAsync(string newEmail, Guid keycloakId, CancellationToken cancellationToken)
    {
        await _accountRepository.UpdateEmailAsync(newEmail, keycloakId, cancellationToken);
    }

    public async Task AddRoleAsync(string roleName, Guid keycloakId, CancellationToken cancellationToken)
    {
        await _accountRepository.AddRoleAsync(roleName, keycloakId, cancellationToken);
    }

    public async Task RemoveRoleAsync(string roleName, Guid keycloakId, CancellationToken cancellationToken)
    {
        await _accountRepository.RemoveRoleAsync(roleName, keycloakId, cancellationToken);
    }

    public async Task UpdateSpecifiedAccountAsync(UpdateAccountDto dto, Guid id, Guid adminKeycloakId, CancellationToken cancellationToken)
    {
        await _accountRepository.HasUserPermissionAsync(id, adminKeycloakId, cancellationToken);
        await _accountRepository.UpdateSpecifiedAccountAsync(dto, id, cancellationToken);
    }

    public async Task UpdateSpecifiedAccountTwoFaAsync(Guid id, Guid adminKeycloakId, CancellationToken cancellationToken)
    {
        await _accountRepository.HasUserPermissionAsync(id, adminKeycloakId, cancellationToken);
        await _accountRepository.UpdateSpecifiedAccountTwoFaAsync(id, cancellationToken);
    }

    public async Task AddSpecifiedAccountRoleAsync(string roleName, Guid id, Guid adminKeycloakId, CancellationToken cancellationToken)
    {
        await _accountRepository.HasUserPermissionAsync(id, adminKeycloakId, cancellationToken);
        await _accountRepository.AddSpecifiedAccountRoleAsync(roleName, id, cancellationToken);
    }

    public async Task RemoveSpecifiedAccountRoleAsync(string roleName, Guid id, Guid adminKeycloakId, CancellationToken cancellationToken)
    {
        await _accountRepository.HasUserPermissionAsync(id, adminKeycloakId, cancellationToken);
        await _accountRepository.RemoveSpecifiedAccountRoleAsync(roleName, id, cancellationToken);
    }

    public async Task<DeletedModel> SoftDeleteAccountAsync(Guid id, Guid adminId, CancellationToken cancellationToken)
    {
        await _accountRepository.HasUserPermissionAsync(id, adminId, cancellationToken);

        var account = await _accountRepository.GetByIdAsync(id, cancellationToken);

        await _accountRepository.SoftDeleteAsync(id, cancellationToken);

        var customerModel = new CustomerChangeModel
        {
            Email = account.Email,
            KeycloakId = account.KeycloakId,
            Type = CustomerChangeType.Delete,
        };
        await _rabbitMQProducerService.SendMessageAsync(customerModel, _queueName, _queueRoutingKey, cancellationToken);

        var deletedModel = new DeletedModel()
        {
            Email = account.Email,
            KeycloakDeletedUserId = account.KeycloakId,
        };
        return deletedModel;
    }

    public async Task<DeletedModel> DeleteAccountAsync(Guid id, Guid adminId, CancellationToken cancellationToken)
    {
        await _accountRepository.HasUserPermissionAsync(id, adminId, cancellationToken);

        var account = await _accountRepository.GetByIdAsync(id, cancellationToken);

        await _accountRepository.DeleteAsync(id, cancellationToken);

        var customerModel = new CustomerChangeModel
        {
            Email = account.Email,
            KeycloakId = account.KeycloakId,
            Type = CustomerChangeType.Delete,
        };
        await _rabbitMQProducerService.SendMessageAsync(customerModel, _queueName, _queueRoutingKey, cancellationToken);

        var deletedModel = new DeletedModel()
        {
            Email = account.Email,
            KeycloakDeletedUserId = account.KeycloakId,
        };
        return deletedModel;
    }

    public async Task<bool> IsTwoFaEnabledAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var dto = await _accountRepository.GetByKeycloakIdAsync(keycloakId, cancellationToken);
        if (dto == null)
            throw new NotFoundException(nameof(dto), keycloakId);

        return dto.IsTwoFaEnabled;
    }

    public async Task VerifyEmailAsync(string email, CancellationToken cancellationToken)
    {
        await _accountRepository.MakeUserVerifiedAsync(email, cancellationToken);
    }

    public async Task<bool> CanUserLoginAsync(string email, CancellationToken cancellationToken)
    {
        var credentials = await _accountRepository.GetCredentialsByEmailAsync(email, cancellationToken);
        if (credentials == null)
        {
            return true;
        }
        if (credentials.HasTempPassword == false) 
        {
            return true;
        }
        else if(credentials.IsPasswordUsed == false && DateTime.UtcNow < credentials.ExpiresAt)
        {
            return true;
        }
        
        return false;
    }

    public async Task MakeTempPasswordUsedAsync(string email, CancellationToken cancellationToken)
    {
        await _accountRepository.MakeTempPasswordUsedAsync(email, cancellationToken);
    }

    public async Task BlockAccountAsync(Guid id, Guid adminId, DateTime? blockUntilUtc, CancellationToken cancellationToken)
    {
        await _accountRepository.HasUserPermissionAsync(id, adminId, cancellationToken);
        await _accountRepository.BlockAccountAsync(id, blockUntilUtc, cancellationToken);
    }

    public async Task UnblockAccountAsync(Guid id, Guid adminId, CancellationToken cancellationToken)
    {
        await _accountRepository.HasUserPermissionAsync(id, adminId, cancellationToken);
        await _accountRepository.UnblockAccountAsync(id, cancellationToken);
    }

    public async Task<TempPasswordModel> CreateTempPassForAccountAsync(Guid id, Guid adminKeycloakId, CancellationToken cancellationToken)
    {
        await _accountRepository.HasUserPermissionAsync(id, adminKeycloakId, cancellationToken);
        var pass = StringGenerator.GeneratePassword();
        await _accountRepository.CreateTempPassForAccountAsync(id, cancellationToken);

        var tempPassModel = new TempPasswordModel 
        { 
            TempPassword= pass
        };
        return tempPassModel;
    }

    public async Task IsUserBlocked(string email, CancellationToken cancellationToken)
    {
        var response = await _accountRepository.GetByEmailAsync(email, cancellationToken);

        if (response.IsBlocked)
        {
            if (response.BlockedUntil.HasValue)
            {
                if (response.BlockedUntil > DateTime.UtcNow)
                {
                    throw new UserBlockedException(email.ToString());
                }
                else
                {
                    await _accountRepository.UnblockAccountAsync(response.Id, cancellationToken);
                    response.IsBlocked = false;
                }
            }
            else
            {
                throw new UserBlockedException(email.ToString());
            }
        }
    }
}
