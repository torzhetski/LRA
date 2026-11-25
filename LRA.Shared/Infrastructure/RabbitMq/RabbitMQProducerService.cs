using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Settings.RabbitMq;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace LRA.Shared.Infrastructure.RabbitMq;

public class RabbitMQProducerService : IRabbitMQProducerService
{
    private readonly IOptions<RabbitMQSettings> _options;

    public RabbitMQProducerService(IOptions<RabbitMQSettings> options)
    {
        _options = options;
    }

    public async Task SendMessageAsync(object obj, string queueName, string rountingKey, CancellationToken token)
    {
        var message = JsonSerializer.Serialize(obj);
        await SendMessageAsync(message, queueName, rountingKey, token);
    }

    public async Task SendMessageAsync(string message, string queueName, string rountingKey, CancellationToken token)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _options.Value.HostName,
            Port = _options.Value.Port,
            UserName = _options.Value.UserName,
            Password = _options.Value.Password,
        };

        using var connection = await factory.CreateConnectionAsync(token);
        using var channel = await connection.CreateChannelAsync(cancellationToken: token);

        await channel.ExchangeDeclareAsync(
            exchange: _options.Value.ExchangeName,
            type: ExchangeType.Direct, 
            durable: true,
            autoDelete: false,
            cancellationToken: token);

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: token);

        await channel.QueueBindAsync(
            queue: queueName,
            exchange: _options.Value.ExchangeName,
            routingKey: rountingKey,
            cancellationToken: token);

        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties
        {
            Persistent = true
        };

        await channel.BasicPublishAsync(
            exchange: _options.Value.ExchangeName,
            routingKey: rountingKey,
            mandatory: true,
            body: body,
            basicProperties: properties,
            cancellationToken: token);
    }
}
