namespace ElectronicsShop.Domain.Settings;

public class EmailSettings
{
    public const string SectionName = "EmailSettings"; 
    
    public string MailServer { get; set; } = string.Empty;
    public int MailPort { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool UseSSL { get; set; }
}