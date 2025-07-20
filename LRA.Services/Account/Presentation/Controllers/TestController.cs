using LRA.Account.Application.Interfaces.Repositories;
using LRA.Shared.Abstractions.Test;
using LRA.Account.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Models;

namespace AccountService.Presentation.Controllers;

[Route("api/test-account")]
public class TestController : ControllerBase
{
    private ITAccountService _testService;
    private readonly IRabbitMQProducerService _rabbitMqProducerService;

    public TestController(ITAccountService testService, IRabbitMQProducerService rabbitMqProducerService)
    {
        _testService = testService;
        _rabbitMqProducerService = rabbitMqProducerService;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInfo([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _testService.GetAccountByIdAsync(id, cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpDelete("soft/{id}")]
    public async Task<IActionResult> SoftDeleteTest([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _testService.SoftDeleteAccount(id, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPost("brocker")]
    public async Task<IActionResult> SendBrockerMessage([FromBody] EmailMessageModel messageModel, CancellationToken cancellationToken)
    {
        await _rabbitMqProducerService.SendMessageAsync(messageModel, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpGet("paginated")]
    public IActionResult GetPaginated([FromQuery] int pageNumber,[FromQuery] int PageSize)
    {
        var response = _testService.GetPaginatedAccounts(pageNumber, PageSize);
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _testService.GetAllAccountsAsync(cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyAsync([FromQuery] string token, [FromQuery] string email, CancellationToken cancellationToken)
    {
        var result = await _testService.VerifyEmailAsync(token, email, cancellationToken);
        if(result)
            return Ok("email verified");
        else
            return BadRequest("email not verified");
    }
}
