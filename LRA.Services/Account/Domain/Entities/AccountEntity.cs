using LRA.Shared.Abstractions.Interfaces.Entities;

namespace LRA.Account.Domain.Entities;

public class AccountEntity : IBaseEntity, ISoftDeleteble
{
    public required Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
    public required string Name { get; set; }
}
