using LRA.Auth.Application.Interfaces;
using LRA.Shared.Abstractions.Test;

namespace LRA.Auth.Application.Services;

public class TestService : ITestService
{
    public TestMessageModel TestResponse(int id)
    {
        var response = new TestMessageModel { Message = $"auth service answered: {id}" };
        return response;
    }
}
