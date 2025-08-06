namespace LRA.Shared.Abstractions.Settings.RabbitMq;

public class RabbitMQSettings
{
    public required string HostName {  get; set; }
    public int Port { get; set; }
    public required string UserName {  get; set; }
    public required string Password { get; set; }
    public required RabbitMqProducerSettings Producer { get; set; }
    public required RabbitMqConsumerSettings Consumer { get; set; }
}
