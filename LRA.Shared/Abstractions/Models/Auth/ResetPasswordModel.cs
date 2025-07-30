
namespace LRA.Shared.Abstractions.Models.Auth;

public class ResetPasswordModel
{
    public required string Email { get; set; }
    public required string NewPassword { get; set; }
    public required string ResetPasswordToken { get; set; }
}
