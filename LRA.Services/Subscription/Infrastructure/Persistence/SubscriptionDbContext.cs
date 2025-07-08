using LRA.Subscription.Infrasturcture.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using LRA.Shared.Abstractions.Test;

namespace LRA.Subscription.Infrasturcture.Persistence;

public class SubscriptionDbContext : DbContext, ISubscriptionDbContext
{
    public SubscriptionDbContext(DbContextOptions<SubscriptionDbContext> options)
       : base(options)
    {
    }

    public virtual DbSet<TestModel> TestModels { get; set; }
}
