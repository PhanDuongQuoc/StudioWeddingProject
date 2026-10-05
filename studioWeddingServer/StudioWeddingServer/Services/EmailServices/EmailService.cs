using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using StudioWeddingServer.DTOs.UserPageDTOs.AuthDTOs;

namespace StudioWeddingServer.Services.EmailServices;

public class EmailService : IEmailService
{
    private readonly EmailSettingDto _settings;

    public EmailService(IOptions<EmailSettingDto> options)
    {
        _settings = options.Value;
    }




    public async Task SendOtpEmailAsync(string toEmail, string otpCode)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
            Subject = "【Hỷ Sự Studio】Mã xác thực đặt lại mật khẩu của bạn",
            Body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 540px; margin: 0 auto; padding: 24px; border: 1px solid #C9C0B2; background-color: #FAF7F0; color: #242421;'>
                    <h2 style='color: #8E2929; text-align: center; margin-bottom: 20px;'>囍 HỶ SỰ WEDDING STUDIO</h2>
                    <p>Xin chào bạn,</p>
                    <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn. Dưới đây là mã xác thực OTP của bạn:</p>
                    <div style='text-align: center; margin: 28px 0;'>
                        <span style='display: inline-block; font-size: 32px; font-weight: bold; letter-spacing: 6px; color: #8E2929; background: #FFFFFF; padding: 12px 28px; border: 2px dashed #8E2929;'>{otpCode}</span>
                    </div>
                    <p style='color: #77736A; font-size: 14px;'>Mã OTP này có hiệu lực trong <strong>10 phút</strong>. Nếu bạn không gửi yêu cầu này, vui lòng bỏ qua email.</p>
                    <hr style='border: none; border-top: 1px solid #C9C0B2; margin: 20px 0;' />
                    <p style='font-size: 12px; color: #77736A; text-align: center;'>Hỷ Sự Wedding Studio — Lưu giữ khoảnh khắc thanh xuân</p>
                </div>
            ",
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        using var client = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort)
        {
            Port = _settings.SmtpPort,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_settings.SenderEmail?.Trim(), _settings.Password?.Trim().Replace(" ", ""))
        };

        await client.SendMailAsync(mailMessage);
    }

    public async Task SendEmailAsync(string toEmail, string customerEmail, string subject, string body)
    {
        var email = new MailMessage();
        email.From = new MailAddress(_settings.SenderEmail, _settings.SenderName);
        email.To.Add(toEmail);
        email.ReplyToList.Add(customerEmail);
        email.Subject = subject;
        email.Body = body;
        email.IsBodyHtml = true;

        var smtpClient = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort)
        {
            Port = _settings.SmtpPort,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_settings.SenderEmail?.Trim(), _settings.Password?.Trim().Replace(" ", ""))
        };

        await smtpClient.SendMailAsync(email);
    }



}
