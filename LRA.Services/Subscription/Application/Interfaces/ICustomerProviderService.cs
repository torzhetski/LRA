
namespace LRA.Subscription.Application.Interfaces;

public interface ICustomerProviderService
{
    Task<string> GetCustomerId(Guid keycloakId, string email, CancellationToken cancellationToken);
}
