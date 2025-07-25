using LRA.Gateways.Client.Extension;
using LRA.Shared.Infrastructure.Settings;
using LRA.Shared.Presentation.Configuration;
using LRA.Shared.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AccountServiceSettings>(
    builder.Configuration.GetSection("AccountServiceSettings"));

builder.Services.Configure<AuthServiceSettings>(
    builder.Configuration.GetSection("AuthServiceSettings"));

builder.Services.Configure<SubscriptionServiceSettings>(
    builder.Configuration.GetSection("SubscriptionServiceSettings"));

builder.Services.AddServices();

builder.Services.AddTypedHttpClients();

builder.Services.AddVersioning();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.WebHost.ConfigureKestrel(options =>
{
    var config = builder.Configuration;

    var httpsPort = int.Parse(config["Https:Port"] ?? "8081");
    var httpPort = int.Parse(config["Http:Port"] ?? "8080");

    var certPath = config["Kestrel:Certificates:Default:Path"];
    var certPassword = config["Kestrel:Certificates:Default:Password"];

    options.ListenAnyIP(httpsPort, listenOptions =>
    {
        listenOptions.UseHttps(certPath, certPassword);
    });

    options.ListenAnyIP(httpPort);
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.AddSwaggerVersioning();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
