using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Domain.Entities;
using LRA.Account.Infrasturcture.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        var emailVerify = new EmailVerificationModel()
        {
            Email = email,
            Token = Guid.NewGuid().ToString(),
        };
        _context.EmailVerification.Add(emailVerify);
        await _context.SaveChangesAsync();
        return emailVerify.Token;
    }

    public async Task<bool> VerifyTokenAsync(string token, string email, CancellationToken cancellationToken)
    {
        var verification = await _context.EmailVerification.FirstOrDefaultAsync(vf => vf.Token == token);
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
