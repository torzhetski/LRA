using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Infrastructure.Persistence.Interfaces;
using LRA.Shared.Utils.Enums;
using LRA.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using LRA.Shared.Utils.Exceptions;
using LRA.Shared.Abstractions.Models.Auth;

namespace LRA.Account.Infrastructure.Persistence.Repositories;

public class VerificationRepository : IVerificationRepository
{
    private readonly IAccountDbContext _context;

    public VerificationRepository(IAccountDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateAsync(string email, VerificationType type, int tokenLifetime, CancellationToken cancellationToken)
    {
        var TwoFaVerify = new VerificationEntity()
        {
            Email = email,
            Token = RandomCode.Generate(),
            Type = type,
            ExpiresAt = DateTime.UtcNow.AddMinutes(tokenLifetime),
        };

        _context.VerificationModels.Add(TwoFaVerify);
        await _context.SaveChangesAsync();

        return TwoFaVerify.Token;
    }

    public async Task<bool> ValidateAsync(string token, string email, VerificationType type ,CancellationToken cancellationToken)
    {
        var response = await _context.VerificationModels.FirstOrDefaultAsync(vf => vf.Token == token, cancellationToken);
        if (response != null && DateTime.UtcNow < response.ExpiresAt && response.Type == type && response.Email == email)
        {
            _context.VerificationModels.Remove(response);
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
