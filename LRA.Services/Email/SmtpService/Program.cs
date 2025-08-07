using LRA.Email.SmtpService.Interfaces;
using LRA.Email.SmtpService.Services;
using LRA.Email.SmtpService.Services.Hosted;
using LRA.Email.SmtpService.Settings;
using LRA.Shared.Abstractions.Settings.RabbitMq;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings"));

builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMqSettings"));

builder.Services.Configure<EmailRabbitMqConsumerSettings>(
    builder.Configuration.GetSection("EmailRabbitMqConsumerSettings"));

builder.Services.AddHostedService<SmtpRabbitMQConsumer>();

builder.Services.AddScoped<IMessageService, SmtpService>();

var host = builder.Build();

host.Run();
