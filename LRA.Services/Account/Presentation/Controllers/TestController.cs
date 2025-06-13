using Microsoft.AspNetCore.Mvc;

namespace AccountService.Presentation.Controllers;
[Route("api/test-account")]
public class TestController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetInfo([FromQuery] int id)
    {
        return Ok(new { message = $"сервис аккаунта ответил {id}" });
    }
}
