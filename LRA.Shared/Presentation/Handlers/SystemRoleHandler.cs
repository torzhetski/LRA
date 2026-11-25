using LRA.Shared.Abstractions.Interfaces;
using LRA.Shared.Presentation.Requirement;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LRA.Gateways.Client.Handlers;

public class SystemRoleHandler : AuthorizationHandler<SystemRoleRequirement>
{
    private readonly IRoleProvider _roleProvider;

    public SystemRoleHandler(IRoleProvider accountService)
    {
        _roleProvider = accountService;
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

        var roles = await _roleProvider.GetRolesById(keycloakId, CancellationToken.None);

        if (roles.Any(role => requirement.AllowedRoles.Contains(role)))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
