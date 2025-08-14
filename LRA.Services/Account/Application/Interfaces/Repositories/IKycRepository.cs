using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Dtos.Kyc;
using System.Globalization;

namespace LRA.Account.Application.Interfaces.Repositories;

public interface IKycRepository
{
    Task CreateIfPossibleAsync(KycEntity entity, CancellationToken cancellationToken);
    Task ApproveKycAsync(Guid userId, CancellationToken cancellationToken);
    Task RejectKycAsync(Guid userId, string rejectMessage, CancellationToken cancellationToken);
    PaginatedList<KycSummaryDto> GetAllPaginatedKyc(int pageNumber, int pageSize);
    PaginatedList<KycSummaryDto> GetInProgressPaginatedKyc(int pageNumber, int pageSize);
    Task<KycDetailsDto> GetKycDetailsAsync(Guid userId, CancellationToken cancellationToken);
}
