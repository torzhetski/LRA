using LRA.Gateways.Client.Interfaces;
using LRA.Gateways.Client.Requirement;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LRA.Gateways.Client.Handlers;

public class SystemRoleHandler : AuthorizationHandler<SystemRoleRequirement>
{
    private readonly IAccountService _accountService;

    public SystemRoleHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SystemRoleRequirement requirement)
    {
        var user = context.User;

        var keycloakId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (keycloakId == null)
        {
            context.Fail();
            return;
        }

        var roles = await _accountService.GetRolesById(keycloakId, CancellationToken.None);

        if (roles.Contains(requirement.RequiredRole))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
