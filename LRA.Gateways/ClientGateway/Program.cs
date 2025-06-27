using LRA.Gateways.Client.Extension;
using LRA.Gateways.Client.Interfaces;
using LRA.Gateways.Client.Services;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Infrastructure.HttpClients;
using LRA.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Options;

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
