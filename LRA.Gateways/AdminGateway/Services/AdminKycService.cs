using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Shared.Abstractions.Dtos.Kyc;
using LRA.Shared.Abstractions.Interfaces.HttpClients;

namespace LRA.Gateways.AdminGateway.Services;

public class AdminKycService : IAdminKycService
{
    private readonly IKycHttpClient _httpClient;

    public AdminKycService(IKycHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task ApproveKycAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _httpClient.ApproveKycAsync(userId, cancellationToken);
    }

    public async Task<PaginatedList<KycSummaryDto>> GetAllPaginatedKyc(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAllPaginatedKyc(pageNumber, pageSize, cancellationToken);
        return response;
    }

    public async Task<PaginatedList<KycSummaryDto>> GetInProgressPaginatedKyc(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetInProgressPaginatedKyc(pageNumber, pageSize, cancellationToken);
        return response;
    }

    public async Task<KycDetailsDto> GetKycDetailsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetKycDetailsAsync(userId, cancellationToken);
        return response;
    }

    public async Task RejectKycAsync(Guid userId, string rejectMessage, CancellationToken cancellationToken)
    {
        await _httpClient.RejectKycAsync(userId, rejectMessage, cancellationToken);
    }
}
