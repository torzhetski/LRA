using Microsoft.AspNetCore.Authorization;

namespace LRA.Gateways.Client.Requirement;

public class SystemRoleRequirement : IAuthorizationRequirement
{
    public string RequiredRole { get; }

    public SystemRoleRequirement(string requiredRole)
    {
        RequiredRole = requiredRole;
    }
}
