using LRA.Shared.Abstractions.Test;

namespace LRA.Subscription.Application.Interfaces;

public interface ISubscriptionService
{
    TestMessageModel TestResponse(int id);
}
