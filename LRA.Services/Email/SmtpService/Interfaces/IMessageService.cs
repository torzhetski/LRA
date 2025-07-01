using LRA.Shared.Abstractions.Models;

namespace LRA.Email.SmtpService.Interfaces;

public interface IMessageService
{
    Task SendAsync(EmailMessageModel messageModel);
}
