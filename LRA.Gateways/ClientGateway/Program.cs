using LRA.Gateways.Client.Interfaces;
using LRA.Gateways.Client.Options;
using LRA.Gateways.Client.Services;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Infrastructure.HttpClients;
using LRA.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ServiceUrlSettings>(
    builder.Configuration.GetSection("ServiceUrl"));

builder.Services.Configure<ApiKeySetting>(
    builder.Configuration.GetSection("ApiKeys"));

builder.Services.AddScoped<ITestAccountService, TestAccountService>();
builder.Services.AddScoped<ITestAuthService, TestAuthService>();
builder.Services.AddScoped<ITestSubscriptionService, TestSubscriptionService>();

builder.Services.AddHttpClient<IAccountHttpClient, AccountHttpClient>((provider, client) =>
{
    var options = provider.GetRequiredService<IOptions<ServiceUrlSettings>>().Value;
    client.BaseAddress = new Uri(options.BaseAccountUrl);
});

builder.Services.AddHttpClient<IAuthHttpClient, AuthHttpClient>((provider, client) =>
{
    var options = provider.GetRequiredService<IOptions<ServiceUrlSettings>>().Value;
    client.BaseAddress = new Uri(options.BaseAuthUrl);
});

builder.Services.AddHttpClient<ISubscriptionHttpClient, SubscriptionHttpClient>((provider, client) =>
{
    var options = provider.GetRequiredService<IOptions<ServiceUrlSettings>>().Value;
    client.BaseAddress = new Uri(options.BaseSubscriptionUrl);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8081, listenOptions =>
    {
        try
        {
            listenOptions.UseHttps("/https/LRA.sertificate.pfx", "1234");
        }
        catch (Exception ex)
        {
            Console.WriteLine("HTTPS error: " + ex.Message);
        }
    });

    options.ListenAnyIP(8080);
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
