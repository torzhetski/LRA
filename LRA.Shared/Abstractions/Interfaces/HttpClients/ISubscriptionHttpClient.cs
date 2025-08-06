using LRA.Shared.Abstractions.Models;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface ISubscriptionHttpClient
{
    Task<RedirectUrlModel> GetChekoutSessionAsync(string keycloakId, string email, CancellationToken cancellationToken);
    Task<RedirectUrlModel> GetCustomerPortalSessionAsync(string keycloakId, string email, CancellationToken cancellationToken);
}
