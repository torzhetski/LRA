using LRA.Shared.Abstractions.RabbitMQProvider.Services;
using LRA.Shared.Abstractions.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace LRA.Account.Application.Services.Hosted;

public class AccountRabbitMQConsumer : RabbitMQConsumerBase
{
    private readonly ILogger<AccountRabbitMQConsumer> _logger;

    public AccountRabbitMQConsumer(IOptions<RabbitMQSettings> options, ILogger<AccountRabbitMQConsumer> logger) : base(options)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        try
        {
            token.ThrowIfCancellationRequested();
        }
        catch (OperationCanceledException ex) 
        {
            _logger.LogError(ex, ex.Message);
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());

            Console.WriteLine($"Message received: {content}");

            await _channel.BasicAckAsync(ea.DeliveryTag, false, token);
        };

        await _channel.BasicConsumeAsync(
            _options.Value.QueueName,
            false,
            consumer,
            token);
    }

    public override void Dispose()
    {
        _connection?.Dispose();
        _channel?.Dispose();
        base.Dispose();
    }
}
