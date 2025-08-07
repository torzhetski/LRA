namespace LRA.Subscription.Domain.Settings;

public class SubscriptionRabbitMqConsumerSettings
{
    public required string WebhookQueue { get; set; }
    public required string CustomerUpdateQueue { get; set; }
}
