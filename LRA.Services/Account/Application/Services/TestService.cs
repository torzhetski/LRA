using LRA.Account.Application.Interfaces;
using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Test;
using LRA.Account.Domain.Mappers;

namespace LRA.Account.Application.Services;

public class TestService : ITestService
{
    private readonly IAccountRepository _accountRepository;

    public TestService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Guid> CreateAccountAsync(AccountEntity model, CancellationToken cancellationToken)
    {
        var id = await _accountRepository.CreateAsync(model);
        await _accountRepository.SaveChangesAsync(cancellationToken);
        return id;
    }

    public async Task<AccountDto> GetAccountById(Guid id, CancellationToken cancellationToken)
    {
        var response = await _accountRepository.GetAsync(id, cancellationToken);

        return response;
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
}
