namespace LRA.Email.SmtpService.Interfaces;

public interface IMessageService
{
    Task SendAsync(string text, string recipientMail);
}
