using LRA.Account.Infrasturcture.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using LRA.Shared.Abstractions.Test;
using LRA.Account.Domain.Entities;

namespace LRA.Account.Infrasturcture.Persistence;

public class AccountDbContext : DbContext, IAccountDbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options)
       : base(options)
    {
    }

    public virtual DbSet<AccountEntity> Accounts { get; set; }
    public virtual DbSet<EmailVerificationModel> EmailVerification { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountEntity>().HasQueryFilter(e => !e.IsDeleted);
    }
}
