
namespace LRA.Subscription.Application.Interfaces;

public interface ICustomerProviderService
{
    Task<string> GetCustomerId(Guid keycloakId, CancellationToken cancellationToken);
    Task CreateStripeCustomer(Guid keycloakId, string email, CancellationToken cancellationToken);
    Task DeleteStripeCustomer(Guid keycloakId, CancellationToken cancellationToken);
}
