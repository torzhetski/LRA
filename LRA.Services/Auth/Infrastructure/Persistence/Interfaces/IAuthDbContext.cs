using LRA.Shared.Abstractions.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace LRA.Auth.Infrasturcture.Persistence.Interfaces;

public interface IAuthDbContext
{
    DbSet<VerificationModel> TwoFaVerificationModels { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
