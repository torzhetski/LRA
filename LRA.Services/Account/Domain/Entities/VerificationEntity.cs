using LRA.Shared.Utils.Enums;

namespace LRA.Shared.Abstractions.Models.Auth;

public class VerificationEntity
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public required VerificationType Type { get; set; }
}
