using LRA.Account.Application.Interfaces;
using LRA.Shared.Abstractions.Test;

namespace LRA.Account.Application.Services;

public class TestService : ITestService
{
    public TestMessageModel TestResponse(int id)
    {
        var response = new TestMessageModel { Message = $"account service answered: {id}" };
        return response;
    }
}
