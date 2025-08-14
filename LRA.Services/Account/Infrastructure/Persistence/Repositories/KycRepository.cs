using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Domain.Entities;
using LRA.Account.Domain.Enums;
using LRA.Account.Infrastructure.Persistence.Interfaces;
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
}
