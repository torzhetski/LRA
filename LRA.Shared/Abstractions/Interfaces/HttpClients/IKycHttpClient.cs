using LRA.Shared.Abstractions.Dtos.Kyc;
using LRA.Shared.Abstractions.Models;
using Microsoft.AspNetCore.Http;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IKycHttpClient
{
    Task ApplyToJobAsync(string id, IFormFile identityFile, IFormFile certificationFile, IFormFile selfieFile, CancellationToken cancellationToken);
    Task DeletePhotoAsync(string fileName, CancellationToken cancellationToken);
    Task ApproveKycAsync(Guid userId, CancellationToken cancellationToken);
    Task RejectKycAsync(Guid userId, string rejectMessage, CancellationToken cancellationToken);
    Task<PaginatedList<KycSummaryDto>> GetAllPaginatedKyc(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<PaginatedList<KycSummaryDto>> GetInProgressPaginatedKyc(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<KycDetailsDto> GetKycDetailsAsync(Guid userId, CancellationToken cancellationToken);
}
