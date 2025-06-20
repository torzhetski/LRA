using LRA.Shared.Abstractions.Test;

namespace LRA.Auth.Application.Interfaces;

public interface ITestService
{
    TestMessageModel TestResponse(int id);
}
