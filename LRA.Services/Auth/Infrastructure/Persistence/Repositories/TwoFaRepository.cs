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

public class TwoFaRepository : ITwoFaRepository
{
    private readonly IAuthDbContext _context;

    public TwoFaRepository(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task<TwoFaDto> CreateAsync(string email, string jwtToken, CancellationToken cancellationToken)
    {
        var TwoFaVerify = new TwoFactorVerificationEntity()
        {
            Email = email,
            Token = RandomCode.Generate(),
            Type = VerificationType.TwoFaVerification,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            JwtToken = jwtToken,
        };

        _context.TwoFaVerificationModels.Add(TwoFaVerify);
        await _context.SaveChangesAsync();

        return new TwoFaDto
        {
            Id = TwoFaVerify.Id,
            Token = TwoFaVerify.Token,
        };
    }

    public async Task<string> ValidateAsync(string token, Guid id, CancellationToken cancellationToken)
    {
        var response = await _context.TwoFaVerificationModels.FirstOrDefaultAsync(vf => vf.Id == id, cancellationToken);
        if (response != null && DateTime.UtcNow < response.ExpiresAt && response.Token == token)
        {
            _context.TwoFaVerificationModels.Remove(response);
            return response.JwtToken;
        }
        else
        {
            throw new NotFoundException(nameof(response), id);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
