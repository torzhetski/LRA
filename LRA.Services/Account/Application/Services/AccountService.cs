using LRA.Account.Application.Interfaces;
using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Utils.Exceptions;

namespace LRA.Account.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task CreateAccountAsync(string email, Guid keycloakId, CancellationToken cancellationToken)
    {
        if (!await _accountRepository.IsAccountExists(keycloakId, cancellationToken))
        {
            var model = new AccountEntity()
            {
                KeycloakId = keycloakId,
                Email = email,
            };
            var id = await _accountRepository.CreateAsync(model, cancellationToken);
        }
    }

    public async Task<AccountDto> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _accountRepository.GetByIdAsync(id, cancellationToken);

        return response;
    }

    public async Task<AccountDto> GetAccountByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var response = await _accountRepository.GetByEmailAsync(email, cancellationToken);
        return response;
    }

    public async Task<AccountDto> GetAccountByKeycloakIdAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var response = await _accountRepository.GetByKeycloakIdAsync(keycloakId, cancellationToken);

        return response;
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

    public async Task SoftDeleteAccountAsync(Guid id, CancellationToken cancellationToken)
    {
        await _accountRepository.SoftDeleteAsync(id, cancellationToken);
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
}
