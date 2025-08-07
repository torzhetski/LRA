using LRA.Shared.Abstractions.Interfaces.Entities;

namespace LRA.Subscription.Domain.Entities;

public class SubscriptionEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public required string StripeSubscriptionId { get; set; }
    public string? CustomerId { get; set; }
    public string? PriceId { get; set; }
    public string? Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool CancelAtThePeriodEnd { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? LastUpdated { get; set; }
}
