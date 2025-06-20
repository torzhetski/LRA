using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Services;

public class TestSubscriptionService : ITestSubscriptionService
{
    private readonly ISubscriptionHttpClient _httpClient;

    public TestSubscriptionService(ISubscriptionHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TestMessageModel> TestSubscriptionServiceAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(id, cancellationToken);
        return response;
    }
}
