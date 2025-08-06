using LRA.Subscription.Infrasturcture.Persistence.Extension;
using LRA.Shared.Presentation.Middlewares;
using LRA.Subscription.Application.Extension;
using LRA.Subscription.Infrasturcture.External.StripeProvider.Extension;
using LRA.Subscription.Infrastructure.Persistence.Extensions;
using LRA.Subscription.Infrasturcture.External.StripeProvider.Settings;
using LRA.Subscription.Domain.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<StripeSettings>(
    builder.Configuration.GetSection("StripeSettings"));

builder.Services.Configure<PriceSettings>(
    builder.Configuration.GetSection("StripePrice"));

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddStripeServices();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrations();
}

app.UseMiddleware<ApiKeyValidationMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
