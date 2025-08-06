namespace LRA.Shared.Abstractions.Dtos.Account;

public class AccountDto
{
    public required Guid Id { get; set; }
    public Guid KeycloakId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime? UpdatedUtc { get; set; }
    public bool IsTwoFaEnabled { get; set; } = true;
}
