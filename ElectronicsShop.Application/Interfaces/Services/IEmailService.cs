namespace ElectronicsShop.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string message);
    
}