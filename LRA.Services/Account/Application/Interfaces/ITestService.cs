using Shared.Abstractions.Test;

namespace LRA.Account.Application.Interfaces;

public interface ITestService
{
    TestMessageModel TestResponse(int id);
}
