namespace LRA.Shared.Abstractions.Interfaces.Entities;

public interface ISoftDeleteble
{
    bool IsDeleted { get; set; }

    DateTime? DeletedOnUtc { get; set; }
}
