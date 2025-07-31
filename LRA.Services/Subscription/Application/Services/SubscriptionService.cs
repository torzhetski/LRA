using LRA.Subscription.Application.Interfaces;
using LRA.Shared.Abstractions.Test;

namespace LRA.Subscription.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    public TestMessageModel TestResponse(int id)
    {
        var response = new TestMessageModel { Message = $"subscription service answered: {id}" };
        return response;
    }
}
