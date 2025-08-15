using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Gateways.AdminGateway.Services;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Dtos.Kyc;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LRA.Gateways.AdminGateway.Controllers;

[Route("api/kyc")]
[ApiController]
public class KycController : ControllerBase
{
    private readonly IAdminKycService _adminKycService;

    public KycController(IAdminKycService adminKycService)
    {
        _adminKycService = adminKycService;
    }

    [HttpPost("approve")]
    public async Task<IActionResult> ApproveKyc([FromQuery] Guid applicationId, CancellationToken cancellationToken)
    {
        await _adminKycService.ApproveKycAsync(applicationId, cancellationToken);
        return Ok();
    }

    [HttpPost("reject")]
    public async Task<IActionResult> RejectKyc([FromQuery] Guid applicationId, string rejectMessage, CancellationToken cancellationToken)
    {
        await _adminKycService.RejectKycAsync(applicationId, rejectMessage, cancellationToken);
        return Ok();
    }

    [HttpGet("paginated/all")]
    public async Task<IActionResult> GetAllKyc([FromQuery] int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var result = await _adminKycService.GetAllPaginatedKyc(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    [HttpGet("paginated/in-progress")]
    public async Task<IActionResult> GetInProgressKyc([FromQuery] int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var result = await _adminKycService.GetInProgressPaginatedKyc(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    [HttpGet("specified")]
    public async Task<IActionResult> GetKycDetails([FromQuery] Guid applicationId, CancellationToken cancellationToken)
    {
        var result = await _adminKycService.GetKycDetailsAsync(applicationId, cancellationToken);
        return Ok(result);
    }
}
