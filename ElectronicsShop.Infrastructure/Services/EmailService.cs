using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ElectronicsShop.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    // The incorrect dependencies have been removed.
    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    // The return type is now Task, and the try-catch is gone.
    public async Task SendEmailAsync(string recipientEmail, string subject, string htmlBody)
    {
        using var client = new SmtpClient();
        
        // Let exceptions bubble up for proper logging.
        await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort);
        await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);

        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
        mailMessage.To.Add(new MailboxAddress("", recipientEmail));
        mailMessage.Subject = subject;
        mailMessage.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

        await client.SendAsync(mailMessage);
        await client.DisconnectAsync(true);
        
    }
}