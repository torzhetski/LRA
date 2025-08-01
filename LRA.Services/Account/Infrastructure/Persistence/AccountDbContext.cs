using LRA.Account.Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Utils.Constants;

namespace LRA.Account.Infrastructure.Persistence;

public class AccountDbContext : DbContext, IAccountDbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options)
       : base(options)
    {
    }

    public virtual DbSet<AccountEntity> Accounts { get; set; }
    public virtual DbSet<VerificationEntity> VerificationModels { get; set; }
    public virtual DbSet<RoleEntity> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AccountEntity>().HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<RoleEntity>()
            .HasIndex(r => r.RoleName)
            .IsUnique();

        modelBuilder.Entity<AccountEntity>()
            .HasMany(a => a.Roles)
            .WithMany(r => r.Accounts)
            .UsingEntity(j => j.ToTable("AccountRoles"));

        modelBuilder.Entity<RoleEntity>().HasData(
            new RoleEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleName = SystemRoles.SuperAdmin },
            new RoleEntity { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), RoleName = SystemRoles.Admin },
            new RoleEntity { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), RoleName = SystemRoles.Client },
            new RoleEntity { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), RoleName = SystemRoles.Paramedic }
        );
    }
}
