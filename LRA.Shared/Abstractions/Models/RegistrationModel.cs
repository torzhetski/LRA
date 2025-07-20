namespace LRA.Shared.Abstractions.Models;

public class RegistrationModel
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
}
