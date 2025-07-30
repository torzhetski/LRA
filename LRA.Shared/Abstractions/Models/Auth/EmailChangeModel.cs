namespace LRA.Shared.Abstractions.Models.Auth;

public class EmailChangeModel
{
    public required string NewEmail { get; set; }
    public required string EmailChangeToken { get; set; }
}
