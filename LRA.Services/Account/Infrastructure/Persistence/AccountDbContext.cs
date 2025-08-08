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

        modelBuilder.Entity<AccountEntity>()
            .HasOne(e => e.CredentialsDetails)
            .WithOne()
            .HasForeignKey<AccountEntity>(e => e.CredentialsId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RoleEntity>().HasData(
            new RoleEntity { Id = Guid.Parse("7f2e843e-1ffb-4ef5-b13c-1eecf50df577"), RoleName = SystemRoles.SuperAdmin },
            new RoleEntity { Id = Guid.Parse("f496959f-8e1a-4a4f-b5d2-49f32c6e82af"), RoleName = SystemRoles.Admin },
            new RoleEntity { Id = Guid.Parse("9d773595-f07e-4c44-a00e-b5bdc388ba11"), RoleName = SystemRoles.Client },
            new RoleEntity { Id = Guid.Parse("39c39491-7a96-4e2d-831e-d38def097117"), RoleName = SystemRoles.Paramedic }
        );
    }
}
