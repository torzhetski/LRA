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
        private readonly ILogger<SmtpRabbitMQConsumer> _logger;

        public SmtpRabbitMQConsumer(
            IOptions<RabbitMQSettings> options,
            IServiceProvider serviceProvider,
            ILogger<SmtpRabbitMQConsumer> logger)
            : base(options)
        {
            _serviceProvider = serviceProvider;
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
                var messageModel = JsonSerializer.Deserialize<EmailMessageModel>(content);

                await SendEmailAsync(messageModel);

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

        private async Task SendEmailAsync(EmailMessageModel messageModel)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var messageService = scope.ServiceProvider.GetService<IMessageService>();
                await messageService.SendAsync(messageModel.Content, messageModel.RecipientMail);
            }
        }
    }
}
