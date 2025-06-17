using LRA.Auth.Infrasturcture.Persistence.Exstensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();