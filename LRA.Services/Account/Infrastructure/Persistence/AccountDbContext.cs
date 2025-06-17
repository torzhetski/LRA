using LRA.Account.Infrasturcture.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using LRA.Shared.Abstructions.Test;

namespace LRA.Account.Infrasturcture.Persistence;

public class AccountDbContext : DbContext, IAccountDbContext
{

    public AccountDbContext(DbContextOptions<AccountDbContext> options)
       : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public virtual DbSet<TestModel> TestModels { get; set; }
}
