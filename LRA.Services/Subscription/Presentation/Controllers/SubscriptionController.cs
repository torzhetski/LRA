using LRA.Subscription.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LRA.Subscription.Presentation.Controllers;

[Route("api/subscription")]
[ApiController]
public class SubscriptionController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public SubscriptionController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [Authorize]
    [HttpGet("chekout-session")]
    public async Task<IActionResult> GetChekoutSession([FromQuery] Guid keycloakId, string email, CancellationToken cancellationToken)
    {
        var url = await _paymentService.GetCheckoutSessionUrlAsync(keycloakId, email, cancellationToken);
        return Ok(url);
    }

    [Authorize]
    [HttpGet("customers-portal-session")]
    public async Task<IActionResult> GetCustomersPortalSession([FromQuery] Guid keycloakId, string email, CancellationToken cancellationToken)
    {
        var url = await _paymentService.GetCustomersPortalUrlAsync(keycloakId, email, cancellationToken);
        return Ok(url);
    }
}
