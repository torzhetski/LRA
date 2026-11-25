namespace LRA.Shared.Abstractions.Models.Auth;

public class PasswordChangeModel
{
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
}
