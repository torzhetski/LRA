using LRA.Subscription.Infrasturcture.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using LRA.Shared.Abstructions.Test;

namespace LRA.Subscription.Infrasturcture.Persistence;

public class SubscriptionDbContext : DbContext, ISubscriptionDbContext
{

    public SubscriptionDbContext(DbContextOptions<SubscriptionDbContext> options)
       : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public virtual DbSet<TestModel> TestModels { get; set; }
}
