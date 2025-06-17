using LRA.Shared.Abstructions.Test;
using Microsoft.EntityFrameworkCore;

namespace LRA.Account.Infrasturcture.Persistence.Interfaces;

public interface IAccountDbContext
{
    DbSet<TestModel> TestModels { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
