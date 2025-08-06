namespace LRA.Shared.Abstractions.Models.Auth;

public class RegistrationModel
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
