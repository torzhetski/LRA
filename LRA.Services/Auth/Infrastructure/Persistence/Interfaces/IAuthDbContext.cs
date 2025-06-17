using LRA.Shared.Abstructions.Test;
using Microsoft.EntityFrameworkCore;

namespace LRA.Auth.Infrasturcture.Persistence.Interfaces;

public interface IAuthDbContext
{
    DbSet<TestModel> TestModels { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
