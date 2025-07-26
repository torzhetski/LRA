using LRA.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LRA.Auth.Infrasturcture.Persistence.Interfaces;

public interface IAuthDbContext
{
    DbSet<TwoFactorVerificationEntity> TwoFaVerificationModels { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
