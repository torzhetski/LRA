using LRA.Shared.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstractions.Test;

namespace LRA.Subscription.Presentation.Controllers;

[Route("api/test-subscription")]
[ApiController]
public class TestController : ControllerBase
{
    [Authorize(Roles = SystemRoles.Client)]
    [HttpGet]
    public async Task<IActionResult> GetInfo([FromQuery] int id)
    {
        return Ok(new TestMessageModel { Message = $"subscription service answered: {id}" });
    }
}
