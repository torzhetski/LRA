using Shared.Abstractions.Interfaces;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IHttpClientService, HttpClientService>();

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