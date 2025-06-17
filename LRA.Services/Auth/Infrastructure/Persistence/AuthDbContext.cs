using LRA.Auth.Infrasturcture.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using LRA.Shared.Abstructions.Test;

namespace LRA.Auth.Infrasturcture.Persistence;

public class AuthDbContext : DbContext, IAuthDbContext
{

    public AuthDbContext(DbContextOptions<AuthDbContext> options)
       : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public virtual DbSet<TestModel> TestModels { get; set; }
}
