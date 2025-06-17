using LRA.Account.Infrasturcture.Persistence.Repositories;
using LRA.Shared.Abstructions.Test;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Presentation.Controllers;
[Route("api/test-account")]
public class TestController : ControllerBase
{
    private readonly AccountRepository _accountRepository;

    public TestController(AccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetInfo([FromQuery] int id)
    {
        return Ok(new { message = $"сервис аккаунта ответил {id}" });
    }

    [HttpPost]
    public async Task<IActionResult> CreateTest([FromBody] TestModel model)
    {
        var id = _accountRepository.Create(model);
        await _accountRepository.SaveChangesAsync();
        return Ok(new { message = $"новый id: {id}" });
    }
}
