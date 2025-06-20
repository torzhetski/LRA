using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Interfaces;

public interface ITestSubscriptionService
{
    Task<TestMessageModel> TestSubscriptionServiceAsync(int id, CancellationToken cancellationToken);
}
