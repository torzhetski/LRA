using LRA.Account.Application.Interfaces;
using LRA.Account.Application.Interfaces.Repositories;
using LRA.Shared.Abstractions.Test;

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

    public async Task<TestMessageModel> TestCreateDataResponse(TestModel model, CancellationToken cancellationToken)
    {
        var id = _accountRepository.CreateAsync(model);
        await _accountRepository.SaveChangesAsync(cancellationToken);
        var response = new TestMessageModel() { Message = $"new aacount id: {id}" };
        return response;
    }

    public async Task<TestDto> TestGetDataResponse(Guid id, CancellationToken cancellationToken)
    {
        var response = await _accountRepository.GetAsync(id, cancellationToken);

        return response.FromModelToDto();
    }
}
