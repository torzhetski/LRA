using LRA.Auth.Infrasturcture.Persistence.Exstensions;
using LRA.Auth.Application.Extension;
using LRA.Shared.Presentation.Middlewares;

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
