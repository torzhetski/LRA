using LRA.Account.Application.Interfaces.Repositories;
using LRA.Shared.Abstractions.Test;
using LRA.Account.Application.Interfaces;
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
    [HttpGet("/{id}")]
    public async Task<IActionResult> GetInfo([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = _testService.TestGetDataResponse(id, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTest([FromBody] TestDto model, CancellationToken cancellationToken)
    {
        var response = await _testService.TestCreateDataResponse(model.FromDtoToModel(), cancellationToken);
        return Ok(response);
    }
}
