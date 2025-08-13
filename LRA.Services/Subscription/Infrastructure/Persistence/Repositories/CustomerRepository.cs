using LRA.Shared.Utils.Exceptions;
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
        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), keycloakId);
        }
        return entity;
    }

    public async Task<CustomerEntity?> GetKeycloakIdOrDefaultByCustomerAysnc(string customerId, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers.FirstOrDefaultAsync(x => x.StripeCustomerId == customerId, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), customerId);
        }
        return entity;
    }

    public async Task DeleteCustomerAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers.FirstOrDefaultAsync(x => x.KeycloakId == keycloakId, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), keycloakId);
        }
        _context.Customers.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
