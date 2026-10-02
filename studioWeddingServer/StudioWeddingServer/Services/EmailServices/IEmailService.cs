namespace StudioWeddingServer.Services.EmailServices;

public interface IEmailService
{
    Task SendOtpEmailAsync(string toEmail, string otpCode);
}
