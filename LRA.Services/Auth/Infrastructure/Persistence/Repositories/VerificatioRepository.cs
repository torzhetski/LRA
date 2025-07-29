using LRA.Auth.Application.Interfaces.Repositories;
using LRA.Auth.Infrasturcture.Persistence.Interfaces;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Abstractions.Test;
using LRA.Shared.Utils.Enums;
using LRA.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using LRA.Auth.Domain.Dtos;
using LRA.Auth.Domain.Entities;
using LRA.Shared.Utils.Exceptions;

namespace LRA.Auth.Infrasturcture.Persistence.Repositories;

public class VerificatioRepository : IVerificationRepository
{
    private readonly IAuthDbContext _context;

    public VerificatioRepository(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task<VerificationTokenDto> CreateTwoFactorAsync(string email, string jwtToken, CancellationToken cancellationToken)
    {
        var TwoFaVerify = new TwoFactorVerificationEntity()
        {
            Email = email,
            Token = RandomCode.Generate(),
            Type = VerificationType.TwoFaVerification,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            JwtToken = jwtToken,
        };

        _context.VerificationModels.Add(TwoFaVerify);
        await _context.SaveChangesAsync();

        return new VerificationTokenDto
        {
            Id = TwoFaVerify.Id,
            Token = TwoFaVerify.Token,
        };
    }

    public async Task<string> ValidateTwoFactorAsync(string token, CancellationToken cancellationToken)
    {
        var response = await _context.VerificationModels.OfType<TwoFactorVerificationEntity>().FirstOrDefaultAsync(vf => vf.Token == token, cancellationToken);
        if (response != null && DateTime.UtcNow < response.ExpiresAt && response.Token == token)
        {
            _context.VerificationModels.Remove(response);
            return response.JwtToken;
        }
        else
        {
            throw new NotFoundException(nameof(response), token);
        }
    }

    public async Task<VerificationTokenDto> CreatePasswordResetAsync(string email, CancellationToken cancellationToken)
    {
        var passwordReset = new VerificationModel()
        {
            Email = email,
            Token = RandomCode.Generate(),
            Type = VerificationType.ResetPassword,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
        };

        _context.VerificationModels.Add(passwordReset);
        await _context.SaveChangesAsync();

        return new VerificationTokenDto
        {
            Id = passwordReset.Id,
            Token = passwordReset.Token,
        };
    }

    public async Task ValidatePasswordResetAsync(string token, CancellationToken cancellationToken)
    {
        var response = await _context.VerificationModels.FirstOrDefaultAsync(vf => vf.Token == token, cancellationToken);
        if (response != null && DateTime.UtcNow < response.ExpiresAt && response.Token == token)
        {
            _context.VerificationModels.Remove(response);;
        }
        else
        {
            throw new NotFoundException(nameof(response), token);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
