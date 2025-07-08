using LRA.Shared.Abstractions.Test;

namespace LRA.Subscription.Application.Interfaces;

public interface ITestService
{
    TestMessageModel TestResponse(int id);
}
