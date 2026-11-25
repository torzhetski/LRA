namespace LRA.Shared.Abstractions.Dtos.Account;

public class AccountDetailsDto
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
    public bool IsDeleted { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool? UsedTempPassword { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime? BlockedUntil { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}
