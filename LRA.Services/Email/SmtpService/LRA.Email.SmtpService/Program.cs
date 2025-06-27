using LRA.Email.SmtpService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<EmailService>();

var host = builder.Build();
host.Run();
