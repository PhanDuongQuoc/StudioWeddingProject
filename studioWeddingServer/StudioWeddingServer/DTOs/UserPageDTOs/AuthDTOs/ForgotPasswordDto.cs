namespace StudioWeddingServer.DTOs.UserPageDTOs.AuthDTOs;

public class SendForgotOtpRequest
{
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordWithOtpRequest
{
    public string Email { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class ForgotPasswordResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
