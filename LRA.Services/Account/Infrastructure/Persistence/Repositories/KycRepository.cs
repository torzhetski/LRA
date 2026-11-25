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

    public async Task ApproveKycAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        var entity = await _context.Kycs.FirstOrDefaultAsync(x => x.Id == applicationId && x.Status == KycStatus.InProcess);
        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), applicationId);
        }

        entity.Status = KycStatus.Approved;
        await _context.SaveChangesAsync(cancellationToken);
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

    public async Task RejectKycAsync(Guid applicationId, string rejectMessage, CancellationToken cancellationToken)
    {
        var entity = await _context.Kycs.FirstOrDefaultAsync(x => x.Id == applicationId && x.Status == KycStatus.InProcess);
        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), applicationId);
        }

        entity.Status = KycStatus.Rejected;
        entity.RejectMessage = rejectMessage;
        await _context.SaveChangesAsync(cancellationToken);
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
                JobApplicationId = kyc.Id,
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
            JobApplicationId = x.kyc.Id,
            AccountId = x.account.Id,
            Email = x.account.Email,
            Status = x.kyc.Status.ToString(),
        });

        return new PaginatedList<KycSummaryDto>(query, pageNumber, pageSize);
    }

    public async Task<KycDetailsDto> GetKycDetailsAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        var result = await _context.Accounts
        .Join(
            _context.Kycs,
            account => account.Id,
            kyc => kyc.AccountId,
            (account, kyc) => new { account, kyc }
        )
        .Where(x => x.kyc.Id == applicationId)
        .Select(x => new KycDetailsDto
        {
            JobApplicationId = x.kyc.Id,
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
            throw new NotFoundException(nameof(result), applicationId);
        }

        return result;
    }
}
