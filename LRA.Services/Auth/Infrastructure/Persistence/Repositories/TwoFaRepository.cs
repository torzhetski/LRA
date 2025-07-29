using LRA.Auth.Application.Interfaces.Repositories;
using LRA.Auth.Infrasturcture.Persistence.Interfaces;
using LRA.Shared.Utils.Enums;
using LRA.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using LRA.Shared.Utils.Exceptions;
using LRA.Shared.Abstractions.Models.Auth;

namespace LRA.Auth.Infrasturcture.Persistence.Repositories;

public class TwoFaRepository : ITwoFaRepository
{
    private readonly IAuthDbContext _context;

    public TwoFaRepository(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateAsync(string email, CancellationToken cancellationToken)
    {
        var TwoFaVerify = new VerificationModel()
        {
            Email = email,
            Token = RandomCode.Generate(),
            Type = VerificationType.TwoFaVerification,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
        };

        _context.VerificationModels.Add(TwoFaVerify);
        await _context.SaveChangesAsync();

        return TwoFaVerify.Token;
    }

    public async Task<bool> ValidateAsync(string token, CancellationToken cancellationToken)
    {
        var response = await _context.VerificationModels.FirstOrDefaultAsync(vf => vf.Token == token, cancellationToken);
        if (response != null && DateTime.UtcNow < response.ExpiresAt && response.Type == VerificationType.TwoFaVerification)
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
