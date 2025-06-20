using LRA.Account.Application.Interfaces;
using LRA.Account.Application.Interfaces.Repositories;
using LRA.Shared.Abstructions.Test;
using System.Reflection;

namespace LRA.Account.Application.Services;

public class TestService : ITestService
{
    private readonly IAccountRepository _accountRepository;

    public TestService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public TestMessageModel TestResponse(int id)
    {
        var response = new TestMessageModel { Message = $"account service answered: {id}" };
        return response;
    }

    public async Task<TestMessageModel> TestDatabaseResponse(TestModel model, CancellationToken cancellationToken)
    {
        var id = _accountRepository.Create(model);
        await _accountRepository.SaveChangesAsync(cancellationToken);
        return new TestMessageModel() { Message = $"new aacount id: {id}" };
    }
}
