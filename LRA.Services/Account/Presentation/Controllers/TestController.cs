using LRA.Account.Application.Interfaces.Repositories;
using LRA.Shared.Abstructions.Test;
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
    [HttpGet]
    public async Task<IActionResult> GetInfo([FromQuery] int id)
    {
        var response = _testService.TestResponse(id);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTest([FromBody] TestModel model)
    {
        var id = _accountRepository.Create(model);
        await _accountRepository.SaveChangesAsync();
        return Ok(new { message = $"новый id: {id}" });
    }
}
