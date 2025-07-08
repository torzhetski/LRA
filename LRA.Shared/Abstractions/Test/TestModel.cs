using LRA.Shared.Abstractions.Interfaces.Entities;

namespace LRA.Shared.Abstractions.Test;

public class TestModel : IBaseEntity, ISoftDeleteble
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
}
