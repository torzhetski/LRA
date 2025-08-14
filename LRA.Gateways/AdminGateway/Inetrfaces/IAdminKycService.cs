using LRA.Shared.Abstractions.Dtos.Kyc;

namespace LRA.Gateways.AdminGateway.Inetrfaces;

public interface IAdminKycService
{
    Task ApproveKycAsync(Guid userId, CancellationToken cancellationToken);
    Task RejectKycAsync(Guid userId, string rejectMessage, CancellationToken cancellationToken);
    Task<PaginatedList<KycSummaryDto>> GetAllPaginatedKyc(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<PaginatedList<KycSummaryDto>> GetInProgressPaginatedKyc(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<KycDetailsDto> GetKycDetailsAsync(Guid userId, CancellationToken cancellationToken);
}
