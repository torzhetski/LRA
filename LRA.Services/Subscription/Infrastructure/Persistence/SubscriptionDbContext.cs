using LRA.Subscription.Infrasturcture.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using LRA.Subscription.Domain.Entities;

namespace LRA.Subscription.Infrasturcture.Persistence;

public class SubscriptionDbContext : DbContext, ISubscriptionDbContext
{
    public SubscriptionDbContext(DbContextOptions<SubscriptionDbContext> options)
       : base(options)
    {
    }

    public virtual DbSet<CustomerEntity> Customers { get; set; }
    public virtual DbSet<SubscriptionEntity> Subscriptions { get; set; }
}
