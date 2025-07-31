using LRA.Subscription.Infrasturcture.Persistence.Extension;
using LRA.Shared.Presentation.Middlewares;
using LRA.Subscription.Application.Extension;
using LRA.Subscription.Infrasturcture.External.StripeProvider.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddStripeServices();

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ApiKeyValidationMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
