using LRA.Shared.Abstractions.Models;
using Microsoft.Extensions.Primitives;

namespace LRA.Subscription.Application.Interfaces;

public interface IPaymentService
{
    Task<RedirectUrlModel> GetCheckoutSessionUrlAsync(Guid keycloakId, CancellationToken cancellationToken);
    Task<RedirectUrlModel> GetCustomersPortalUrlAsync(Guid keycloakId, CancellationToken cancellationToken);
}
