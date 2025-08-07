using LRA.Shared.Abstractions.Interfaces.Entities;

namespace LRA.Account.Domain.Entities;

public class CredentialsDetailsEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public bool IsPasswordUsed { get; set; }
    public required DateTime ExpiresAt { get; set; }
}
