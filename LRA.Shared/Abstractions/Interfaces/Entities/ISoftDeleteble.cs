namespace LRA.Shared.Abstractions.Interfaces.Entities;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }

    DateTime? DeletedOnUtc { get; set; }
}
