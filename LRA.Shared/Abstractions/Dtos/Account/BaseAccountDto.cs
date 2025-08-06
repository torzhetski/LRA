namespace LRA.Shared.Abstractions.Dtos.Account;

public class BaseAccountDto
{
    public required Guid KeycloakId { get; set; }
    public required string Email { get; set; }
}
