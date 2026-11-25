using LRA.Subscription.Infrasturcture.Persistence.Extension;
using LRA.Shared.Presentation.Middlewares;
using LRA.Subscription.Application.Extension;
using LRA.Subscription.Infrasturcture.External.StripeProvider.Extension;
using LRA.Subscription.Infrastructure.Persistence.Extensions;
using LRA.Subscription.Infrasturcture.External.StripeProvider.Settings;
using LRA.Subscription.Domain.Settings;
using LRA.Shared.Abstractions.Settings.RabbitMq;
using LRA.Shared.Presentation.Handlers;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Infrastructure.RabbitMq;
using LRA.Subscription.Application.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<StripeSettings>(
    builder.Configuration.GetSection("StripeSettings"));

builder.Services.Configure<PriceSettings>(
    builder.Configuration.GetSection("StripePrice"));

builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMqSettings"));

builder.Services.Configure<SubscriptionRabbitMqProducerSettings>(
    builder.Configuration.GetSection("SubscriptionRabbitMqProducerSettings"));

builder.Services.Configure<SubscriptionRabbitMqConsumerSettings>(
    builder.Configuration.GetSection("SubscriptionRabbitMqConsumerSettings"));

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddStripeServices();

builder.Services.AddSingleton<IRabbitMQProducerService, RabbitMQProducerService>();

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<BaseExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrations();
}

app.UseExceptionHandler();

app.UseMiddleware<ApiKeyValidationMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
