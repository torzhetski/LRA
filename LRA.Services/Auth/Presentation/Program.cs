using LRA.Auth.Infrasturcture.Persistence.Exstensions;
using LRA.Auth.Application.Extension;
using LRA.Shared.Presentation.Middlewares;
using LRA.Shared.Infrastructure.Settings;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Infrastructure.HttpClients;
using Microsoft.Extensions.Options;
using LRA.Auth.Presentation.Interfaces;
using LRA.Auth.Presentaion.Services;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Infrastructure.RabbitMq;
using LRA.Shared.Abstractions.Settings.RabbitMq;
using LRA.Auth.Presentation.Handlers;
using LRA.Shared.Presentation.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KeycloakServiceSettings>(
    builder.Configuration.GetSection("KeycloakServiceSettings"));

builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMqSettings"));

builder.Services.AddHttpClient<IKeycloakHttpClient, KeycloakHttpClient>();

builder.Services.AddScoped<IKeycklockService, KeycklockService>();

builder.Services.AddSingleton<IRabbitMQProducerService, RabbitMQProducerService>();

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<BaseExceptionHandler>();
builder.Services.AddExceptionHandler<AuthExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.UseMiddleware<ApiKeyValidationMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
