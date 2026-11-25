using LRA.Shared.Abstractions.Dtos.Kyc;

namespace LRA.Gateways.AdminGateway.Inetrfaces;

public interface IAdminKycService
{
    Task ApproveKycAsync(Guid applicationId, CancellationToken cancellationToken);
    Task RejectKycAsync(Guid applicationId, string rejectMessage, CancellationToken cancellationToken);
    Task<PaginatedList<KycSummaryDto>> GetAllPaginatedKyc(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<PaginatedList<KycSummaryDto>> GetInProgressPaginatedKyc(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<KycDetailsDto> GetKycDetailsAsync(Guid applicationId, CancellationToken cancellationToken);
}
