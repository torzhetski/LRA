using LRA.Shared.Abstractions.Models;

namespace LRA.Gateways.Client.Interfaces;

public interface ISubscriptionService
{
    Task<RedirectUrlModel> GetChekoutSessionAsync(string keycloakId, CancellationToken cancellationToken);
    Task<RedirectUrlModel> GetCustomersPortalUrlAsync(string keycloakId, CancellationToken cancellationToken);
}
