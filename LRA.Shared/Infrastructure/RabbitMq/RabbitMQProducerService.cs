using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Settings;
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

    public async Task SendMessageAsync(object obj, CancellationToken token)
    {
        var json = JsonSerializer.Serialize(obj);
        await SendMessageAsync(json, token);
    }

    public async Task SendMessageAsync(string message, CancellationToken token)
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

        await channel.QueueDeclareAsync(
            queue: _options.Value.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: token);

        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties
        {
            Persistent = true
        };

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: _options.Value.QueueName,
            mandatory: true,
            body: body,
            basicProperties: properties,
            cancellationToken: token);

        Console.WriteLine("Message sent");
    }
}
