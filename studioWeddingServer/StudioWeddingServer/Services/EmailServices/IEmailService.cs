namespace StudioWeddingServer.Services.EmailServices;

public interface IEmailService
{
    Task SendOtpEmailAsync(string toEmail, string otpCode);
    Task SendEmailAsync(string toEmail, string customerEmail, string subject, string body);
}
