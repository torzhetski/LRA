using LRA.Shared.Abstractions.Models;
using Microsoft.Extensions.Primitives;

namespace LRA.Subscription.Application.Interfaces;

public interface IPaymentService
{
    Task<RedirectUrlModel> GetCheckoutSessionUrlAsync(Guid keycloakId, string email, CancellationToken cancellationToken);
    Task<RedirectUrlModel> GetCustomersPortalUrlAsync(Guid keycloakId, string email, CancellationToken cancellationToken);
}
