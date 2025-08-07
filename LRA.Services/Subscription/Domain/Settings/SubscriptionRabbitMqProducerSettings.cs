namespace LRA.Subscription.Domain.Settings;

public class SubscriptionRabbitMqProducerSettings
{
    public required string RoleUpdateQueue {  get; set; }
    public required string RoleUpdateQueueRoutingKey { get; set; }
}
