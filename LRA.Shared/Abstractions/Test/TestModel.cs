using LRA.Shared.Abstractions.Interfaces.Entities;

namespace LRA.Shared.Abstractions.Test;

public class TestModel : IBaseEntity
{
    public required Guid Id { get; set; }
    public string? Name { get; set; }
}
