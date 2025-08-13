namespace LRA.Account.Domain.Models;

public class DeletedModel
{
    public required Guid KeycloakDeletedUserId { get; set; }
    public string? Email { get; set; }
}
