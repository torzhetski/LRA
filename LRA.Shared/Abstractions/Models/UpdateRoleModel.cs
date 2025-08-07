using LRA.Shared.Utils.Enums;

namespace LRA.Shared.Abstractions.Models;

public class UpdateRoleModel
{
    public required Guid KeycloakId { get; set; }
    public required string RoleName { get; set; }
    public required RoleUpdateType UpdateType { get; set; }
}
