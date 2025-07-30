using LRA.Account.Application.Extension;
using LRA.Account.Infrastructure.Persistence.Extensions;
using LRA.Account.Presentation.Handlers;
using LRA.Account.Presentation.Interfaces;
using LRA.Account.Presentation.Services;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Settings.RabbitMq;
using LRA.Shared.Infrastructure.HttpClients;
using LRA.Shared.Infrastructure.RabbitMq;
using LRA.Shared.Infrastructure.Settings;
using LRA.Shared.Presentation.Handlers;
using LRA.Shared.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMqSettings"));

builder.Services.Configure<KeycloakServiceSettings>(
    builder.Configuration.GetSection("KeycloakServiceSettings"));

builder.Services.AddHttpClient<IKeycloakHttpClient, KeycloakHttpClient>();

builder.Services.AddScoped<IKeycloakService, KeycloakService>();

builder.Services.AddSingleton<IRabbitMQProducerService, RabbitMQProducerService>();

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<BaseExceptionHandler>();
builder.Services.AddExceptionHandler<AccountExceptionHandler>();
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
