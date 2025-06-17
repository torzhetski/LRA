using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstractions.Test;

namespace AuthService.Presentation.Controllers;

[Route("api/test-auth")]
public class TestController : ControllerBase
{
    [Authorize(Roles = "Client")]
    [HttpGet]
    public async Task<IActionResult> GetInfo([FromQuery] int id)
    {
        return Ok(new TestMessageModel{ Message = $"authentication service answered: {id}" });
    }
}
