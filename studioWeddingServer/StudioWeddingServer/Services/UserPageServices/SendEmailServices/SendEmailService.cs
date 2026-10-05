using StudioWeddingServer.Services.EmailServices;
using StudioWeddingServer.Services.UserPageServices.EmailServices;

namespace StudioWeddingServer.Services.UserPageServices.SendEmailServices;

public class SendEmailService : ISendEmailService
{
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public SendEmailService(IEmailService emailService, IConfiguration configuration)
    {
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task<ContactRespone> SendEmailAsync(ContactRequest request)
    {
        var toEmail = _configuration["EmailSettings:SenderEmail"] ?? "contact@hysustudio.vn";
        var customerEmail = request.EmailCustomer;
        var subject = $"[HỶ SỰ STUDIO] Khách hàng {request.NameCutomer} gửi yêu cầu tư vấn";
        var body = getBody(request);

        await _emailService.SendEmailAsync(toEmail, customerEmail, subject, body);

        return new ContactRespone
        {
            Status = true,
            Message = "Email đã được gửi thành công"
        };
    }

    public string getBody(ContactRequest request)
    {
        var weddingDateFormatted = request.DataWedding != default
            ? request.DataWedding.ToString("dd/MM/yyyy")
            : "Chưa xác định";

        var customerNote = !string.IsNullOrWhiteSpace(request.Note)
            ? request.Note.Replace("\n", "<br/>")
            : "<em>(Khách hàng không để lại ghi chú thêm)</em>";

        var body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            </head>

            <body style='margin:0; padding:0; background-color:#f5f3f0; font-family:Arial, Helvetica, sans-serif;'>

                <table width='100%' cellpadding='0' cellspacing='0' border='0'
                    style='background-color:#f5f3f0; padding:40px 20px;'>
                    <tr>
                        <td align='center'>

                            <!-- Main Card -->
                            <table width='600' cellpadding='0' cellspacing='0' border='0'
                                style='max-width:600px; background-color:#ffffff; border-radius:12px; overflow:hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.08);'>

                                <!-- Header -->
                                <tr>
                                    <td style='background-color:#8E2929; padding:35px 40px; text-align:center;'>

                                        <div style='font-size:13px; letter-spacing:3px; color:#f3d9b1; text-transform:uppercase;'>
                                            囍 HỶ SỰ WEDDING STUDIO
                                        </div>

                                        <h1 style='margin:12px 0 5px 0; color:#ffffff; font-size:24px; font-weight:600;'>
                                            Yêu Cầu Tư Vấn Mới
                                        </h1>

                                        <p style='margin:0; color:#f8e8d8; font-size:14px;'>
                                            Bạn vừa nhận được thông tin liên hệ từ khách hàng qua website
                                        </p>

                                    </td>
                                </tr>

                                <!-- Content -->
                                <tr>
                                    <td style='padding:35px 40px;'>

                                        <p style='margin:0 0 16px 0; color:#444444; font-size:15px;'>
                                            Xin chào Studio,
                                        </p>

                                        <p style='margin:0 0 25px 0; color:#555555; font-size:15px; line-height:1.6;'>
                                            Khách hàng <strong>{request.NameCutomer}</strong> vừa gửi thông tin liên hệ yêu cầu tư vấn gói dịch vụ cưới:
                                        </p>

                                        <!-- Customer Information Table -->
                                        <table width='100%' cellpadding='0' cellspacing='0' border='0'
                                            style='border:1px solid #e8e3dd; border-radius:8px; overflow:hidden;'>

                                            <tr>
                                                <td colspan='2'
                                                    style='padding:14px 18px; background-color:#faf8f5; border-bottom:1px solid #e8e3dd;'>
                                                    <strong style='font-size:15px; color:#2f2a27;'>
                                                        📋 Thông tin khách hàng
                                                    </strong>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td width='38%'
                                                    style='padding:12px 18px; color:#777777; font-size:14px; border-bottom:1px solid #f0ece6;'>
                                                    Họ và tên
                                                </td>

                                                <td style='padding:12px 18px; color:#222222; font-size:14px; font-weight:600; border-bottom:1px solid #f0ece6;'>
                                                    {request.NameCutomer}
                                                </td>
                                            </tr>

                                            <tr>
                                                <td width='38%'
                                                    style='padding:12px 18px; color:#777777; font-size:14px; border-bottom:1px solid #f0ece6;'>
                                                    Số điện thoại
                                                </td>

                                                <td style='padding:12px 18px; color:#222222; font-size:14px; font-weight:bold; border-bottom:1px solid #f0ece6;'>
                                                    <a href='tel:{request.PhoneNumber}' style='color:#8E2929; text-decoration:none;'>
                                                        {request.PhoneNumber}
                                                    </a>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td width='38%'
                                                    style='padding:12px 18px; color:#777777; font-size:14px; border-bottom:1px solid #f0ece6;'>
                                                    Địa chỉ Email
                                                </td>

                                                <td style='padding:12px 18px; font-size:14px; border-bottom:1px solid #f0ece6;'>
                                                    <a href='mailto:{request.EmailCustomer}'
                                                    style='color:#8E2929; text-decoration:none;'>
                                                        {request.EmailCustomer}
                                                    </a>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td width='38%'
                                                    style='padding:12px 18px; color:#777777; font-size:14px;'>
                                                    Ngày cưới dự kiến
                                                </td>

                                                <td style='padding:12px 18px; color:#222222; font-size:14px;'>
                                                    📅 {weddingDateFormatted}
                                                </td>
                                            </tr>

                                        </table>

                                        <!-- Message Section (ĐÃ BỔ SUNG ĐẦY ĐỦ Ở ĐÂY) -->
                                        <div style='margin-top:28px;'>
                                            <p style='margin:0 0 10px 0; color:#2f2a27; font-size:15px; font-weight:bold;'>
                                                💬 Nội dung cần tư vấn:
                                            </p>
                                            <div style='background-color:#faf8f5; border:1px solid #e8e3dd; border-left:4px solid #8E2929; border-radius:6px; padding:16px 18px; color:#333333; font-size:14px; line-height:1.6;'>
                                                {customerNote}
                                            </div>
                                        </div>

                                        <!-- Reply Button -->
                                        <div style='text-align:center; margin-top:32px;'>
                                            <a href='mailto:{request.EmailCustomer}'
                                            style='display:inline-block;
                                                    background-color:#8E2929;
                                                    color:#ffffff;
                                                    padding:12px 28px;
                                                    border-radius:6px;
                                                    text-decoration:none;
                                                    font-weight:600;
                                                    font-size:14px;'>
                                                ✉️ Phản hồi khách hàng ngay
                                            </a>
                                        </div>

                                    </td>
                                </tr>

                                <!-- Footer -->
                                <tr>
                                    <td style='background-color:#faf8f5; padding:20px 40px; text-align:center; border-top:1px solid #eee8e0;'>
                                        <p style='margin:0 0 6px 0; color:#555555; font-size:13px; font-weight:600;'>
                                            Hỷ Sự Wedding Studio
                                        </p>
                                        <p style='margin:0; color:#999999; font-size:12px;'>
                                            Email này được gửi tự động từ hệ thống website Hỷ Sự Wedding Studio.</p>
                                    </td>
                                </tr>

                            </table>

                        </td>
                    </tr>
                </table>

            </body>
            </html>";

        return body;
    }
}
