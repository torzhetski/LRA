using LRA.Account.Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Models.Auth;

namespace LRA.Account.Infrastructure.Persistence;

public class AccountDbContext : DbContext, IAccountDbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options)
       : base(options)
    {
    }

    public virtual DbSet<AccountEntity> Accounts { get; set; }
    public virtual DbSet<VerificationModel> EmailVerification { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountEntity>().HasQueryFilter(e => !e.IsDeleted);
    }
}
