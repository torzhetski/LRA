using LRA.Shared.Utils.Enums;

namespace LRA.Shared.Abstractions.Models.Stripe;

public class CustomerChangeModel
{
    public required Guid KeycloakId { get; set; }
    public required string Email { get; set; }
    public required CustomerChangeType Type { get; set; }
}
