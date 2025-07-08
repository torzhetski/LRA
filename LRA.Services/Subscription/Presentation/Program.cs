using LRA.Subscription.Infrasturcture.Persistence.Exstensions;
using LRA.Shared.Presentation.Middlewares;
using LRA.Subscription.Application.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ApiKeyValidationMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
