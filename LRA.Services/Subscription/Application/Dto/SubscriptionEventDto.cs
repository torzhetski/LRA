using LRA.Subscription.Domain.Entities;

namespace LRA.Subscription.Application.Dto;

public class SubscriptionEventDto
{
    public required SubscriptionEntity Entity { get; set; }
    public required string EventType { get; set; }
}
