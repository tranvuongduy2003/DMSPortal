using DMSPortal.Models.Common;

namespace DMSPortal.BackendServer.Abstractions.Services;

public interface IEmailService
{
    Task SendMail(MailContent mailContent);

    Task SendEmailAsync(string email, string subject, string htmlMessage);
}