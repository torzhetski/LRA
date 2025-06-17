using Microsoft.AspNetCore.Mvc;

namespace AuthService.Presentation.Controllers;

[Route("api/test-auth")]
public class TestController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetInfo([FromQuery] int id)
    {
        return Ok(new { message = $"authentication service answered: {id}" });
    }
}
