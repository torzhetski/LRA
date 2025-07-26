using LRA.Shared.Abstractions.Interfaces.Entities;

namespace LRA.Account.Domain.Entities;

public class AccountEntity : IBaseEntity, ISoftDeleteble
{
    public Guid Id { get; set; }
    public required Guid KeycloakId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedUtc { get; set; }
    public bool IsEmailVerified { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
    public bool Is2FaEnabled { get; set; } = true;
}
