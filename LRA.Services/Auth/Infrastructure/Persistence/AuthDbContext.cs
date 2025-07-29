using LRA.Auth.Infrasturcture.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using LRA.Shared.Abstractions.Test;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Auth.Domain.Entities;

namespace LRA.Auth.Infrasturcture.Persistence;

public class AuthDbContext : DbContext, IAuthDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
       : base(options)
    {
    }

    public virtual DbSet<VerificationModel> VerificationModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VerificationModel>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<VerificationModel>("ResetPassword")
            .HasValue<TwoFactorVerificationEntity>("TwoFactorVerification");
        base.OnModelCreating(modelBuilder);
    }
}
