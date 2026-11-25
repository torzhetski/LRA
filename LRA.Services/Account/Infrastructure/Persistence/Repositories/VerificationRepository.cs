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
        var verify = new VerificationEntity()
        {
            Email = email,
            Token = StringGenerator.GenerateCode(),
            Type = type,
            IsUsed = false,
            ExpiresAt = DateTime.UtcNow.AddMinutes(tokenLifetime),
        };

        _context.VerificationModels.Add(verify);
        await _context.SaveChangesAsync();

        return verify.Token;
    }

    public async Task<bool> ValidateAsync(string token, string email, VerificationType type ,CancellationToken cancellationToken)
    {
        var response = await _context.VerificationModels.FirstOrDefaultAsync(vf => 
            vf.Token == token &&
            DateTime.UtcNow < vf.ExpiresAt &&
            vf.Type == type &&
            vf.Email == email &&
            vf.IsUsed == false, 
            cancellationToken);

        if (response != null )
        {
            response.IsUsed = true;
            await SaveChangesAsync(cancellationToken);
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
