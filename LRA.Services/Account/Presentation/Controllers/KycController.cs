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

    public KycController(IAzureService azureService)
    {
        _azureService = azureService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UploadPhoto([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        using var stream = file.OpenReadStream();
        var url = await _azureService.UploadPhotoAsync(stream, file.FileName, cancellationToken);

        return Ok(url);
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> DeletePhoto([FromRoute] string fileName, CancellationToken cancellationToken)
    {
        await _azureService.DeletePhotoAsync(fileName, cancellationToken);
        return Ok();
    }
}
