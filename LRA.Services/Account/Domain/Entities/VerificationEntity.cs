using LRA.Shared.Abstractions.Interfaces.Entities;
using LRA.Shared.Utils.Enums;

namespace LRA.Shared.Abstractions.Models.Auth;

public class VerificationEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public required VerificationType Type { get; set; }
}
