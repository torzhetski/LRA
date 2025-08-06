using LRA.Email.SmtpService.Interfaces;
using LRA.Email.SmtpService.Settings;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace LRA.Email.SmtpService.Services;

public class SmtpService : IMessageService
{
    private readonly SmtpClient _smtpClient;
    private readonly IOptions<SmtpSettings> _smtpSettings;
    private readonly ILogger<SmtpService> _logger;

    public SmtpService(IOptions<SmtpSettings> smtpSettings, ILogger<SmtpService> logger)
    {
        _smtpSettings = smtpSettings;

        _smtpClient = new SmtpClient(_smtpSettings.Value.Host, _smtpSettings.Value.Port)
        {
            Credentials = new NetworkCredential(_smtpSettings.Value.Username, _smtpSettings.Value.Password),
            EnableSsl = true
        };

        _logger = logger;
    }

    public async Task SendAsync(string text, string recipientMail)
    {
        try
        {
            using (var message = new MailMessage(_smtpSettings.Value.From, recipientMail, "Mail", text))
            {
                await _smtpClient.SendMailAsync(message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
