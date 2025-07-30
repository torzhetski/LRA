using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace LRA.Account.Infrastructure.Persistence.Interfaces;

public interface IAccountDbContext
{
    DbSet<AccountEntity> Accounts { get; set; }
    DbSet<VerificationModel> EmailVerification { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
