using LRA.Account.Application.Interfaces;
using LRA.Shared.Abstractions.Dtos.Kyc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LRA.Account.Presentation.Controllers;

[Route("api/kyc")]
[ApiController]
public class KycController : ControllerBase
{
    private readonly IAzureService _azureService;
    private readonly IKycService _kycService;

    public KycController(IAzureService azureService, IKycService kycService)
    {
        _azureService = azureService;
        _kycService = kycService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ApplyToJob(
        [FromQuery] Guid id, 
        [FromForm] IFormFile identityFile,
        [FromForm] IFormFile certificationFile,
        [FromForm] IFormFile selfieFile, 
        CancellationToken cancellationToken)
    {
        await _kycService.CreateIfPossibleAsync(id, identityFile, certificationFile, selfieFile, cancellationToken);

        return Ok();
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> DeletePhoto([FromRoute] string fileName, CancellationToken cancellationToken)
    {
        await _azureService.DeletePhotoAsync(fileName, cancellationToken);
        return Ok();
    }

    [HttpPost("approve")]
    public async Task<IActionResult> ApproveKyc([FromQuery] Guid applicationId, CancellationToken cancellationToken)
    {
        await _kycService.ApproveKycAsync(applicationId, cancellationToken);
        return NoContent();
    }

    [HttpPost("reject")]
    public async Task<IActionResult> RejectKyc([FromQuery] Guid applicationId, string rejectMessage, CancellationToken cancellationToken)
    {
        await _kycService.RejectKycAsync(applicationId, rejectMessage, cancellationToken);
        return NoContent();
    }

    [HttpGet("paginated/all")]
    public IActionResult GetAllKyc([FromQuery] int pageNumber, int pageSize = 10)
    {
        var result = _kycService.GetAllPaginatedKyc(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("paginated/in-progress")]
    public IActionResult GetInProgressKyc([FromQuery] int pageNumber, int pageSize = 10)
    {
        var result = _kycService.GetInProgressPaginatedKyc(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetKycDetails([FromQuery] Guid applicationId, CancellationToken cancellationToken)
    {
        var result = await _kycService.GetKycDetailsAsync(applicationId, cancellationToken);
        return Ok(result);
    }
}
