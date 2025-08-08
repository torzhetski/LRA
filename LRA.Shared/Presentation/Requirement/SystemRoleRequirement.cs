using Microsoft.AspNetCore.Authorization;

namespace LRA.Shared.Presentation.Requirement;

public class SystemRoleRequirement : IAuthorizationRequirement
{
    public IReadOnlyList<string> AllowedRoles { get; }

    public SystemRoleRequirement(params string[] roles)
    {
        AllowedRoles = roles;
    }
}
