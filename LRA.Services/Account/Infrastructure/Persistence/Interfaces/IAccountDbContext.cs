using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Test;
using Microsoft.EntityFrameworkCore;

namespace LRA.Account.Infrasturcture.Persistence.Interfaces;

public interface IAccountDbContext
{
    DbSet<AccountEntity> Accounts { get; set; }
    DbSet<EmailVerificationModel> EmailVerification { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
