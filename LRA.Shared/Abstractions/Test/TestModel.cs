using LRA.Shared.Abstractions.Interfaces.Entities;

namespace LRA.Shared.Abstractions.Test;

public class TestModel : IBaseEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
