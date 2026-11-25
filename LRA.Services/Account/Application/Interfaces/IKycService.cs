using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Dtos.Kyc;
using Microsoft.AspNetCore.Http;

namespace LRA.Account.Application.Interfaces;

public interface IKycService
{
    Task CreateIfPossibleAsync(Guid userId, IFormFile identityFile, IFormFile certificationFile, IFormFile selfieFile, CancellationToken cancellationToken);
    Task ApproveKycAsync(Guid applicationId, CancellationToken cancellationToken);
    Task RejectKycAsync(Guid applicationId, string rejectMessage, CancellationToken cancellationToken);
    PaginatedList<KycSummaryDto> GetAllPaginatedKyc(int pageNumber, int pageSize);
    PaginatedList<KycSummaryDto> GetInProgressPaginatedKyc(int pageNumber, int pageSize);
    Task<KycDetailsDto> GetKycDetailsAsync(Guid applicationId, CancellationToken cancellationToken);
}
