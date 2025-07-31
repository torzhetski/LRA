namespace LRA.Subscription.Domain.Entities;

public class CustomerEntity
{
    public Guid Id { get; set; }
    public required Guid KeycloakId { get; set; }
    public required string StripeCustomerId { get; set; }
}
