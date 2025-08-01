using LRA.Subscription.Application.Interfaces.Repositories;
using LRA.Subscription.Domain.Entities;
using LRA.Subscription.Infrasturcture.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LRA.Subscription.Infrasturcture.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ISubscriptionDbContext _context;

    public CustomerRepository(ISubscriptionDbContext context)
    {
        _context = context;
    }

    public async Task CreateCustomerAsync(CustomerEntity entity, CancellationToken cancellationToken)
    {
        _context.Customers.Add(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<CustomerEntity?> GetCustomerOrDefaultByKeycloakIdAysnc(Guid keycloakId, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers.FirstOrDefaultAsync(x => x.KeycloakId == keycloakId, cancellationToken);
        return entity;
    }

    public async Task<CustomerEntity?> GetKeycloakIdOrDefaultByCustomerAysnc(string customerId, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers.FirstOrDefaultAsync(x => x.StripeCustomerId == customerId, cancellationToken);
        return entity;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
