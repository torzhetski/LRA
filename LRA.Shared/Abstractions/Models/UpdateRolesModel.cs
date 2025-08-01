namespace LRA.Shared.Abstractions.Models;

public class UpdateRolesModel
{
    public required Guid KeycloakId { get; set; }
    public required List<string> RoleNames { get; set; }
}
