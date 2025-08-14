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
    public async Task<IActionResult> ApplyToJob(
        [FromForm] IFormFile identityFile,
        [FromForm] IFormFile certificationFile,
        [FromForm] IFormFile selfieFile,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }
        await _kycService.ApplyToJobAsync(userId, identityFile, certificationFile, selfieFile, cancellationToken);

        return Ok();
    }
}
