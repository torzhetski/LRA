using LRA.Shared.Utils.Constants;
using LRA.Subscription.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LRA.Shared.Abstractions.Test;

namespace LRA.Subscription.Presentation.Controllers;

[Route("api/test-subscription")]
[ApiController]
public class TestController : ControllerBase
{
    private ITestService _testService;

    public TestController(ITestService testService)
    {
        _testService = testService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetInfo([FromQuery] int id)
    {
        var response = _testService.TestResponse(id);
        return Ok(response);
    }
}
