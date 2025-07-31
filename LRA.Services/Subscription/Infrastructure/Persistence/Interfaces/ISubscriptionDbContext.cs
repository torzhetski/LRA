using LRA.Subscription.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LRA.Subscription.Infrasturcture.Persistence.Interfaces;

public interface ISubscriptionDbContext
{
    DbSet<CustomerEntity> Customers { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
