using LRA.Shared.Abstructions.Test;
using Microsoft.EntityFrameworkCore;

namespace LRA.Subscription.Infrasturcture.Persistence.Interfaces;

public interface ISubscriptionDbContext
{
    DbSet<TestModel> TestModels { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
