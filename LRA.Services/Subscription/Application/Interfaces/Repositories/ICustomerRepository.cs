using LRA.Subscription.Domain.Entities;

namespace LRA.Subscription.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task CreateCustomerAsync(CustomerEntity entity, CancellationToken cancellationToken);
    Task<CustomerEntity?> GetCustomerOrDefaultByKeycloakIdAysnc(Guid keycloakId, CancellationToken cancellationToken);
    Task<CustomerEntity?> GetKeycloakIdOrDefaultByCustomerAysnc(string customerId, CancellationToken cancellationToken);
    Task DeleteCustomerAsync(Guid keycloakId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
