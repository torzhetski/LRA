using LRA.Gateways.Client.Interfaces;
using LRA.Gateways.Client.Options;
using LRA.Gateways.Client.Services;
using Shared.Abstractions.Interfaces;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IHttpClientService, HttpClientService>();

builder.Services.AddScoped<ITestService, TestService>();

builder.Services.Configure<ServiceUrlSettings>(
    builder.Configuration.GetSection("ServiceUrl"));

builder.Services.Configure<ApiKeySetting>(
    builder.Configuration.GetSection("ApiKey"));

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
