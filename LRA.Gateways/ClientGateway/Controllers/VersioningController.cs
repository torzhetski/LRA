using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LRA.Gateways.Client.Controllers
{
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
                return Ok(false);

            if (!int.TryParse(parts[0], out var major))
                return Ok(false);

            int minor = 0;
            if (parts.Length == 2 && !int.TryParse(parts[1], out minor))
                return Ok(false);

            var parsedVersion = new ApiVersion(major, minor);

            var isSupported = _versionProvider.ApiVersionDescriptions
            .Any(d => d.ApiVersion == parsedVersion && !d.IsDeprecated);

            return Ok(isSupported);
        }

        [HttpGet("all")]
        public IActionResult GetAllSupportedVersions()
        {
            var versions = _versionProvider.ApiVersionDescriptions
                .Select(v => v.ApiVersion.ToString())
                .ToList();

            return Ok(versions);
        }
    }
}
