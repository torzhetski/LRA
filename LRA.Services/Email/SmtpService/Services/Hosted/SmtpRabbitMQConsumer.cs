using LRA.Email.SmtpService.Interfaces;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.RabbitMQProvider.Services;
using LRA.Shared.Abstractions.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace LRA.Email.SmtpService.Services.Hosted
{
    public class SmtpRabbitMQConsumer : RabbitMQConsumerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public SmtpRabbitMQConsumer(
            IOptions<RabbitMQSettings> options,
            IServiceProvider serviceProvider)
            : base(options)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                await SendEmailAsync(content);
                Console.WriteLine("message sent");

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

        private async Task SendEmailAsync(string content)
        {
            var messageModel = JsonSerializer.Deserialize<EmailMessageModel>(content);
            using (var scope = _serviceProvider.CreateScope())
            {
                var messageService = scope.ServiceProvider.GetService<IMessageService>();
                await messageService.SendAsync(messageModel);
            }
        }
    }
}
