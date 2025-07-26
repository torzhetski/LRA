using LRA.Shared.Utils.Enums;

namespace LRA.Shared.Abstractions.Models.Auth;

public class VerificationModel
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(1);
    public required VerificationType Type { get; set; }
}
