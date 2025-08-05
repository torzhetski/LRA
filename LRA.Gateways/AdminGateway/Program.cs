using LRA.Gateways.AdminGateway.Extension;
using LRA.Shared.Presentation.Configuration;
using LRA.Shared.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddServices();

builder.Services.AddRolesPolicy();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    
}

app.UseHttpsRedirection();

app.Run();

