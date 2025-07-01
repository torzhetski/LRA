using LRA.Email.SmtpService.Interfaces;
using LRA.Email.SmtpService.Settings;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using LRA.Shared.Abstractions.Models;

namespace LRA.Email.SmtpService.Services;

public class SmtpService : IMessageService
{
    private readonly SmtpClient _smtpClient;
    private readonly IOptions<SmtpSettings> _smtpSettings;

    public SmtpService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings;

        _smtpClient = new SmtpClient(_smtpSettings.Value.Host, _smtpSettings.Value.Port)
        {
            Credentials = new NetworkCredential(_smtpSettings.Value.Username, _smtpSettings.Value.Password),
            EnableSsl = true
        };
    }

    public async Task SendAsync(EmailMessageModel messageModel)
    {
        try
        {
            using (var message = new MailMessage(_smtpSettings.Value.From, messageModel.RecipientMail, "Mail", messageModel.Content))
            {
                await _smtpClient.SendMailAsync(message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
