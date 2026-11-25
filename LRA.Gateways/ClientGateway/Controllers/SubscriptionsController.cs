using Asp.Versioning;
using LRA.Gateways.Client.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LRA.Gateways.Client.Controllers;

[Route("api/v{version:apiVersion}/subscriptions")]
[ApiController]
[ApiVersion("1.0")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionsController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [Authorize]
    [HttpGet("chekout-session")]
    public async Task<IActionResult> GetChekoutSession(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }

        var url = await _subscriptionService.GetChekoutSessionAsync(userId, cancellationToken);
        return Ok(url);
    }

    [Authorize]
    [HttpGet("customers-portal-session")]
    public async Task<IActionResult> GetCustomersPortalSession(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }

        var url = await _subscriptionService.GetCustomersPortalUrlAsync(userId, cancellationToken);
        return Ok(url);
    }
}
