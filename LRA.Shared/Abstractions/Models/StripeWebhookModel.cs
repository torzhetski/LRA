using Microsoft.Extensions.Primitives;

namespace LRA.Shared.Abstractions.Models;

public class StripeWebhookModel
{
    public required string Body { get; set; }
    public required string StripeHeaders { get; set; }
}
