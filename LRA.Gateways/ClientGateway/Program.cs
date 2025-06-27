using LRA.Gateways.Client.Extension;
using LRA.Shared.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ServiceUrlSettings>(
    builder.Configuration.GetSection("ServicesComfiguration:ServiceUrls"));

builder.Services.Configure<ApiKeySetting>(
    builder.Configuration.GetSection("ServicesComfiguration:ApiKeys"));

builder.Services.AddServices();

builder.Services.AddTypedHttpClients();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
