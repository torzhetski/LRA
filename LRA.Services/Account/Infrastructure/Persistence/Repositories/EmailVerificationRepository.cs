using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Infrasturcture.Persistence.Interfaces;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Utils;
using LRA.Shared.Utils.Enums;
using Microsoft.EntityFrameworkCore;

namespace LRA.Account.Infrasturcture.Persistence.Repositories;

public class EmailVerificationRepository : IEmailVerificationRepository
{
    private readonly IAccountDbContext _context;

    public EmailVerificationRepository(IAccountDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateVerificationTokenAsync(string email, CancellationToken cancellationToken)
    {
        var emailVerify = new VerificationModel()
        {
            Email = email,
            Token = RandomCode.Generate(),
            Type = VerificationType.EmailVerification,
        };
        _context.EmailVerification.Add(emailVerify);
        await _context.SaveChangesAsync();
        return emailVerify.Token;
    }

    public async Task<bool> VerifyTokenAsync(string token, string email, CancellationToken cancellationToken)
    {
        var verification = await _context.EmailVerification.FirstOrDefaultAsync(vf => vf.Token == token && vf.Type == VerificationType.EmailVerification);
        if (verification == null || verification.ExpiresAt < DateTime.UtcNow || verification.Email != email)
        {
            return false;
        }
        else
        {
            _context.EmailVerification.Remove(verification);
            return true;    
        }
    }
}
