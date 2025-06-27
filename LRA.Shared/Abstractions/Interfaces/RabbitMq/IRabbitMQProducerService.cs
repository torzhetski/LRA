namespace LRA.Shared.Abstractions.Interfaces.RabbitMq;

public interface IRabbitMQProducerService
{
    Task SendMessageAsync(object obj, CancellationToken token);
    Task SendMessageAsync(string message, CancellationToken token);
}
