using LRA.Gateways.Stripe.Inerfaces;
using LRA.Gateways.Stripe.Services;
using LRA.Gateways.Stripe.Settings;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Abstractions.Settings.RabbitMq;
using LRA.Shared.Infrastructure.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMqSettings"));

builder.Services.Configure<StripeRabbitMqProducerSettings>(
    builder.Configuration.GetSection("StripeRabbitMqProducerSettings"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddScoped<IStripeWebhookService, StripeWebhookService>();

builder.Services.AddSingleton<IRabbitMQProducerService, RabbitMQProducerService>();

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
