using LRA.Account.Infrasturcture.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using LRA.Shared.Abstractions.Test;

namespace LRA.Account.Infrasturcture.Persistence;

public class AccountDbContext : DbContext, IAccountDbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options)
       : base(options)
    {
    }

    public virtual DbSet<TestModel> TestModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestModel>().HasQueryFilter(e => !e.IsDeleted);
    }
}
