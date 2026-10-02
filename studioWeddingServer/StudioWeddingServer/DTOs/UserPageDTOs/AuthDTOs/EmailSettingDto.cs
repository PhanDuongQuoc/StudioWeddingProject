
namespace StudioWeddingServer.DTOs.UserPageDTOs.AuthDTOs;

public class EmailSettingDto
{
    public string SmtpServer { get; set; } = "smtp.gmail.com";
    public int SmtpPort { get; set; } = 587;
    public string SenderName { get; set; } = "Hỷ Sự Wedding Studio";
    public string SenderEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
