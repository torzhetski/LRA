using LRA.Account.Application.Interfaces;
using LRA.Shared.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Presentation.Controllers;

[Route("api/test-account")]
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
