using LRA.Account.Application.Extension;
using LRA.Account.Application.Settings;
using LRA.Account.Infrastructure.Persistence.Extensions;
using LRA.Account.Presentation.Extensions;
using LRA.Account.Presentation.Interfaces;
using LRA.Account.Presentation.Services;
using LRA.Account.Presentation.Settings;
using LRA.Account.Presentation.Utils;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Settings.RabbitMq;
using LRA.Shared.Infrastructure.HttpClients;
using LRA.Shared.Infrastructure.RabbitMq;
using LRA.Shared.Infrastructure.Settings;
using LRA.Shared.Presentation.Middlewares;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMqSettings"));

builder.Services.Configure<AccountRabbitMqConsumerSettings>(
    builder.Configuration.GetSection("AccountRabbitMqConsumerSettings"));

builder.Services.Configure<AccountRabbitMqProducerSettings>(
    builder.Configuration.GetSection("AccountRabbitMqProducerSettings"));

builder.Services.Configure<KeycloakServiceSettings>(
    builder.Configuration.GetSection("KeycloakServiceSettings"));

builder.Services.Configure<AdminSettings>(
    builder.Configuration.GetSection("AdminSettings"));

builder.Services.Configure<KeycloakGoogleProviderSettings>(
    builder.Configuration.GetSection("KeycloakGoogleProviderSettings"));

builder.Services.AddHttpClient<IKeycloakHttpClient, KeycloakHttpClient>();

builder.Services.AddScoped<IKeycloakService, KeycloakService>();

builder.Services.AddSingleton<IRabbitMQProducerService, RabbitMQProducerService>();

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddPresentation();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
    await seeder.SeedAdminIfNotExistsAsync(CancellationToken.None);
    await app.ApplyMigrations();
}

app.UseExceptionHandler();

app.UseMiddleware<ApiKeyValidationMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
