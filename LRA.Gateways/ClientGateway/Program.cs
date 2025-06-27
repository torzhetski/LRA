using LRA.Gateways.Client.Extension;
using LRA.Shared.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AccountServiceSettings>(
    builder.Configuration.GetSection("AccountServiceSettings"));

builder.Services.Configure<AuthServiceSettings>(
    builder.Configuration.GetSection("AuthServiceSettings"));

builder.Services.Configure<SubscriptionServiceSettings>(
    builder.Configuration.GetSection("SubscriptionServiceSettings"));

builder.Services.AddServices();

builder.Services.AddTypedHttpClients();

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
