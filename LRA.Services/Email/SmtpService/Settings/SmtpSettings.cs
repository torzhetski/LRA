namespace LRA.Email.SmtpService.Settings;

public class SmtpSettings
{
    public required string From { get; set; }
    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}
