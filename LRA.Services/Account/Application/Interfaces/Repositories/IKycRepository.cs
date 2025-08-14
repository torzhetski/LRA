using LRA.Account.Domain.Entities;
using System.Globalization;

namespace LRA.Account.Application.Interfaces.Repositories;

public interface IKycRepository
{
    Task CreateIfPossibleAsync(KycEntity entity, CancellationToken cancellationToken);
    Task ApproveKycAsync(Guid userId, CancellationToken cancellationToken);
    Task RejectKycAsync(Guid userId, string rejectMessage, CancellationToken cancellationToken);
}
