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

    public async Task SendMessageAsync(object obj, CancellationToken token)
    {
        var message = JsonSerializer.Serialize(obj);
        await SendMessageAsync(message, token);
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

        await channel.ExchangeDeclareAsync(
            exchange: _options.Value.Producer.ExchangeName,
            type: ExchangeType.Fanout, 
            durable: true,
            autoDelete: false,
            cancellationToken: token);

        await channel.QueueDeclareAsync(
            queue: _options.Value.Producer.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: token);

        await channel.QueueBindAsync(
            queue: _options.Value.Producer.QueueName,
            exchange: _options.Value.Producer.ExchangeName,
            routingKey: "",
            cancellationToken: token);

        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties
        {
            Persistent = true
        };

        await channel.BasicPublishAsync(
            exchange: _options.Value.Producer.ExchangeName,
            routingKey: "",
            mandatory: true,
            body: body,
            basicProperties: properties,
            cancellationToken: token);
    }
}
