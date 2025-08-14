using Asp.Versioning;
using LRA.Gateways.Client.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using System.Security.Claims;

namespace LRA.Gateways.Client.Controllers;

[Route("api/v{version:apiVersion}/kyc")]
[ApiController]
[ApiVersion("1.0")]
public class KycController : ControllerBase
{
    private readonly IKycService _kycService;

    public KycController(IKycService kycService)
    {
        _kycService = kycService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UploadPhoto([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        var response = await _kycService.UploadPhotoAsync(file, cancellationToken);

        return Ok(response);
    }

    [Authorize]
    [HttpDelete("{fileName}")]
    public async Task<IActionResult> DeletePhoto([FromRoute] string fileName, CancellationToken cancellationToken)
    {
        await _kycService.DeletePhotoAsync(fileName, cancellationToken);
        return Ok();
    }
}
