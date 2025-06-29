using LRA.Shared.Abstractions.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Threading.Channels;

namespace LRA.Shared.Abstractions.RabbitMQProvider.Services;

public abstract class RabbitMQConsumerBase : BackgroundService
{
    protected readonly IOptions<RabbitMQSettings> _options;

    protected IConnection _connection;
    protected IChannel _channel;

    public RabbitMQConsumerBase(IOptions<RabbitMQSettings> options)
    {
        _options = options;
    }

    public override async Task StartAsync(CancellationToken token)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _options.Value.HostName,
            Port = _options.Value.Port,
            UserName = _options.Value.UserName,
            Password = _options.Value.Password,
        };
        _connection = await factory.CreateConnectionAsync(token);
        _channel = await _connection.CreateChannelAsync(cancellationToken: token);

        await _channel.ExchangeDeclareAsync(
            exchange: _options.Value.ExchangeName,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            cancellationToken: token);

        await _channel.QueueDeclareAsync(
            queue: _options.Value.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: token);

        await _channel.QueueBindAsync(
            queue: _options.Value.QueueName,
            exchange: _options.Value.ExchangeName,
            routingKey: default,
            cancellationToken: token);

        await base.StartAsync(token);
    }
}
