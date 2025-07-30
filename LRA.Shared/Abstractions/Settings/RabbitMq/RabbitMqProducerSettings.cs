namespace LRA.Shared.Abstractions.Settings.RabbitMq;

public class RabbitMqProducerSettings
{
    public required string ExchangeName { get; set; }
    public required string QueueName { get; set; }
}
