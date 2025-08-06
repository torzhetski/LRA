using LRA.Shared.Abstractions.Interfaces.Entities;

namespace LRA.Subscription.Domain.Entities;

public class CustomerEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public required Guid KeycloakId { get; set; }
    public required string StripeCustomerId { get; set; }
}
