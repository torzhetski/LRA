using LRA.Auth.Infrasturcture.Persistence.Exstensions;
using LRA.Auth.Application.Extension;
using LRA.Shared.Presentation.Middlewares;
using LRA.Shared.Infrastructure.Settings;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Infrastructure.HttpClients;
using Microsoft.Extensions.Options;
using LRA.Auth.Presentation.Interfaces;
using LRA.Auth.Presentaion.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KeycloakServiceSettings>(
    builder.Configuration.GetSection("KeycloakServiceSettings"));

builder.Services.AddHttpClient<IKeycloakHttpClient, KeycloakHttpClient>();

builder.Services.AddScoped<IKeycklockService, KeycklockService>();

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ApiKeyValidationMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
