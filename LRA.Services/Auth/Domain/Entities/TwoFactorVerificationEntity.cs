using LRA.Shared.Abstractions.Models.Auth;

namespace LRA.Auth.Domain.Entities;

public class TwoFactorVerificationEntity : VerificationModel
{
    public required string JwtToken { get; set; }
}
