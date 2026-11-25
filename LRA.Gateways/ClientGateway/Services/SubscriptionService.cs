using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionHttpClient _httpClient;

    public SubscriptionService(ISubscriptionHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RedirectUrlModel> GetChekoutSessionAsync(string keycloakId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetChekoutSessionAsync(keycloakId, cancellationToken);
        return response;
    }

    public async Task<RedirectUrlModel> GetCustomersPortalUrlAsync(string keycloakId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetCustomerPortalSessionAsync(keycloakId, cancellationToken);
        return response;
    }
}
