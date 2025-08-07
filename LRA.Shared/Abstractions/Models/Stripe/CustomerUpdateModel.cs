using LRA.Shared.Utils.Enums;

namespace LRA.Shared.Abstractions.Models.Stripe;

public class CustomerUpdateModel
{
    public required Guid KeycloakId { get; set; }
    public required string Email { get; set; }
}
