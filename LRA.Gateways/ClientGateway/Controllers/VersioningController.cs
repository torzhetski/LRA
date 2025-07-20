using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using LRA.Shared.Abstractions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LRA.Gateways.Client.Controllers;

[Route("api/versioning")]
[ApiController]
[ApiVersionNeutral]
public class VersioningController : ControllerBase
{
    private readonly IApiVersionDescriptionProvider _versionProvider;

    public VersioningController(IApiVersionDescriptionProvider versionProvider)
    {
        _versionProvider = versionProvider;
    }

    [HttpGet("check")]
    public IActionResult CheckVersion([FromQuery] string version)
    {
        var parts = version.Split('.');
        if (parts.Length == 0 || parts.Length > 2)
        {
            return Ok(new VersioningModel
            {
                Version = version,
                IsSupproted = false
            });
        }

        if (!int.TryParse(parts[0], out var major))
        {
            return Ok(new VersioningModel
            {
                Version = version,
                IsSupproted = false
            });
        }

        int minor = 0;
        if (parts.Length == 2 && !int.TryParse(parts[1], out minor))
        {
            return Ok(new VersioningModel
            {
                Version = version,
                IsSupproted = false
            });
        }

        var parsedVersion = new ApiVersion(major, minor);

        var description = _versionProvider.ApiVersionDescriptions
            .FirstOrDefault(d => d.ApiVersion == parsedVersion);

        bool isSupported = description != null && !description.IsDeprecated;

        return Ok(new VersioningModel
        {
            Version = version,
            IsSupproted = isSupported
        });
    }

    [HttpGet("all")]
    public IActionResult GetAllSupportedVersions()
    {
        var versions = _versionProvider.ApiVersionDescriptions
            .Select(v => new VersioningModel
            {
                Version = v.ApiVersion.ToString(),
                IsSupproted = !v.IsDeprecated
            })
            .ToList();

        return Ok(versions);
    }
}
