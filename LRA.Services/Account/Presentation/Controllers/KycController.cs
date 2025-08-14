using LRA.Account.Application.Interfaces;
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
}
