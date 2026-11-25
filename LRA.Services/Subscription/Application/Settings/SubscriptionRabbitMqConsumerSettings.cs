namespace LRA.Subscription.Application.Settings;

public class SubscriptionRabbitMqConsumerSettings
{
    public required string WebhookQueue { get; set; }
    public required string CustomerUpdateQueue { get; set; }
}
