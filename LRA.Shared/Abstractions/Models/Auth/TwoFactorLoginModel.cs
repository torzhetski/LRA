namespace LRA.Shared.Abstractions.Models.Auth;

public class TwoFactorLoginModel : LoginModel
{
    public required string TwoFactorToken { get; set; }
}
