namespace LRA.Account.Application.Settings;

public class AccountRabbitMqProducerSettings
{
    public required string EmailQueue { get; set; }
    public required string EmailQueueRoutingKey { get; set; }
    public required string SubscriptionQueue { get; set; }
    public required string SubscriptionQueueRoutingKey { get; set; }
}
