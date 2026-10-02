using StudioWeddingServer.DTOs.UserPageDTOs.AuthDTOs;

namespace StudioWeddingServer.Services.ForgotServices;

public interface IForgotService
{
    Task<ForgotPasswordResponse> SendForgotOtpAsync(SendForgotOtpRequest request);
    Task<ForgotPasswordResponse> ResetPasswordWithOtpAsync(ResetPasswordWithOtpRequest request);
}
