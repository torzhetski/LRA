namespace LRA.Shared.Abstractions.Dtos.Account;

public class AccountSummaryDto
{
    public required Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
}
