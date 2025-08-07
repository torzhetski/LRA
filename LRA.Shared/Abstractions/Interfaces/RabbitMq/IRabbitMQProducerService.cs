namespace LRA.Shared.Abstractions.Interfaces.RabbitMq;

public interface IRabbitMQProducerService
{
    Task SendMessageAsync(object obj, string queueName, string rountingKey, CancellationToken token);
    Task SendMessageAsync(string message, string queueName, string rountingKey, CancellationToken token);
}
