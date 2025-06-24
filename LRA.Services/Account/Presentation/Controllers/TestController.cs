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
    private readonly IAccountRepository _accountRepository;

    public TestController(ITestService testService,IAccountRepository accountRepository)
    {
        _testService = testService;
        _accountRepository = accountRepository;
    }

    [Authorize]
    [HttpGet("/{id}")]
    public async Task<IActionResult> GetInfo([FromRoute] int id)
    {
        var response = _testService.TestResponse(id);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTest([FromBody] TestModel model, CancellationToken cancellationToken)
    {
        var response = await _testService.TestDatabaseResponse(model, cancellationToken);
        return Ok(response);
    }
}
