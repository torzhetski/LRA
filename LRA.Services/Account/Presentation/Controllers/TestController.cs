using LRA.Account.Application.Interfaces.Repositories;
using LRA.Shared.Abstractions.Test;
using LRA.Account.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;

namespace AccountService.Presentation.Controllers;

[Route("api/test-account")]
public class TestController : ControllerBase
{
    private ITestService _testService;
    private readonly IRabbitMQProducerService _rabbitMqProducerService;

    public TestController(ITestService testService, IRabbitMQProducerService rabbitMqProducerService)
    {
        _testService = testService;
        _rabbitMqProducerService = rabbitMqProducerService;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInfo([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _testService.TestGetDataResponse(id, cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTest([FromBody] TestDto model, CancellationToken cancellationToken)
    {
        var response = await _testService.TestCreateDataResponse(model.FromDtoToModel(), cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> SendbrockerMessage([FromBody] string message, CancellationToken cancellationToken)
    {
        await _rabbitMqProducerService.SendMessageAsync(message, cancellationToken);
        return Ok();
    }
}
