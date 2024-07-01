using DMSPortal.Models.Models;

namespace DMSPortal.BackendServer.Services.Interfaces;

public interface IEmailService
{
    Task SendMail(MailContent mailContent);

    Task SendEmailAsync(string email, string subject, string htmlMessage);
}