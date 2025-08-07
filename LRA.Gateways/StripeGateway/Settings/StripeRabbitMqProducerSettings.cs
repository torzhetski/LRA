namespace LRA.Gateways.Stripe.Settings;

public class StripeRabbitMqProducerSettings
{
    public required string WebhookQueue { get; set; }
    public required string WebhookQueueRoutingKey { get; set; }
}
