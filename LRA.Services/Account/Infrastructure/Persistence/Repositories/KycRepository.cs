using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Domain.Entities;
using LRA.Account.Domain.Enums;
using LRA.Account.Infrastructure.Persistence.Interfaces;
using LRA.Shared.Abstractions.Dtos.Kyc;
using LRA.Shared.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LRA.Account.Infrasturcture.Persistence.Repositories;

public class KycRepository : IKycRepository
{
    private readonly IAccountDbContext _context;

    public KycRepository(IAccountDbContext context)
    {
        _context = context;
    }

    public async Task ApproveKycAsync(Guid userId, CancellationToken cancellationToken)
    {
        var entity = await _context.Kycs.FirstOrDefaultAsync(x => x.AccountId == userId);
        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), userId);
        }

        entity.Status = KycStatus.Approved;
    }

    public async Task CreateIfPossibleAsync(KycEntity entity, CancellationToken cancellationToken)
    {
        var model = await _context.Kycs.FirstOrDefaultAsync(x => x.AccountId == entity.AccountId);
        if (model == null || model.Status == KycStatus.Rejected)
        {
            _context.Kycs.Add(entity);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new AlreadyExistsException(nameof(entity));
        }
    }

    public async Task RejectKycAsync(Guid userId, string rejectMessage, CancellationToken cancellationToken)
    {
        var entity = await _context.Kycs.FirstOrDefaultAsync(x => x.AccountId == userId);
        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), userId);
        }

        entity.Status = KycStatus.Rejected;
        entity.RejectMessage = rejectMessage;
    }

    public PaginatedList<KycSummaryDto> GetAllPaginatedKyc(int pageNumber, int pageSize)
    {
        var query = _context.Accounts
        .Join(
            _context.Kycs,
            account => account.Id,
            kyc => kyc.AccountId,
            (account, kyc) => new KycSummaryDto
            {
                AccountId = account.Id,
                Email = account.Email,
                Status = kyc.Status.ToString()
            }
        );

        return new PaginatedList<KycSummaryDto>(query, pageNumber, pageSize);
    }

    public PaginatedList<KycSummaryDto> GetInProgressPaginatedKyc(int pageNumber, int pageSize)
    {
        var query = _context.Accounts
        .Join(
            _context.Kycs,
            account => account.Id,
            kyc => kyc.AccountId,
            (account, kyc) => new { account, kyc }
        )
        .Where(x => x.kyc.Status == KycStatus.InProcess)
        .Select(x => new KycSummaryDto
        {
            AccountId = x.account.Id,
            Email = x.account.Email,
            Status = x.kyc.Status.ToString(),
        });

        return new PaginatedList<KycSummaryDto>(query, pageNumber, pageSize);
    }

    public async Task<KycDetailsDto> GetKycDetailsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _context.Accounts
        .Join(
            _context.Kycs,
            account => account.Id,
            kyc => kyc.AccountId,
            (account, kyc) => new { account, kyc }
        )
        .Where(x => x.account.Id == userId)
        .Select(x => new KycDetailsDto
        {
            AccountId = x.account.Id,
            Email = x.account.Email,
            FirstName = x.account.FirstName,
            LastName = x.account.LastName,
            IdentityDocumentUrl = x.kyc.IdentityDocumentUrl,
            MedicalCertificationUrl = x.kyc.MedicalCertificationUrl,
            SelfieWithIdentityDocumentUrl = x.kyc.SelfieWithIdentityDocumentUrl,
            RejectMessage = x.kyc.RejectMessage,
            Status = x.kyc.Status.ToString()
        })
        .AsNoTracking()
        .FirstOrDefaultAsync(cancellationToken);

        if(result == null)
        {
            throw new NotFoundException(nameof(result), userId);
        }

        return result;
    }
}
