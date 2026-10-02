using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StudioWeddingServer.DTOs.UserPageDTOs.AuthDTOs;
using StudioWeddingServer.Models;
using StudioWeddingServer.Services.EmailServices;

namespace StudioWeddingServer.Services.ForgotServices;

public class ForgotService : IForgotService
{
    private readonly StudioWeddingDbContext _context;
    private readonly IMemoryCache _memoryCache;
    private readonly IEmailService _emailService;

    public ForgotService(StudioWeddingDbContext context, IMemoryCache memoryCache, IEmailService emailService)
    {
        _context = context;
        _memoryCache = memoryCache;
        _emailService = emailService;
    }


    public async Task<ForgotPasswordResponse> SendForgotOtpAsync(SendForgotOtpRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return new ForgotPasswordResponse { Success = false, Message = "Email không được để trống" };
        }

        var cleanEmail = request.Email.Trim().ToLower();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == cleanEmail);

        if (user == null)
        {
            return new ForgotPasswordResponse { Success = false, Message = "Không tìm thấy tài khoản với Email này" };
        }

        if (user.IsActive == false)
        {
            return new ForgotPasswordResponse { Success = false, Message = "Tài khoản này đã bị khóa hoặc ngừng hoạt động" };
        }

        var otpCode = Random.Shared.Next(100000, 999999).ToString();

        var cacheKey = $"FORGOT_OTP_{cleanEmail}";
        _memoryCache.Set(cacheKey, otpCode, TimeSpan.FromMinutes(10));

        try
        {
            await _emailService.SendOtpEmailAsync(cleanEmail, otpCode);
        }
        catch (Exception ex)
        {
            var detail = ex.InnerException != null ? $"{ex.Message} ({ex.InnerException.Message})" : ex.Message;
            return new ForgotPasswordResponse
            {
                Success = false,
                Message = $"Gửi email thất bại: {detail}. Vui lòng kiểm tra lại cấu hình tài khoản/mật khẩu ứng dụng trong appsettings.json."
            };
        }

        return new ForgotPasswordResponse
        {
            Success = true,
            Message = "Mã xác thực OTP đã được gửi về email của bạn. Vui lòng kiểm tra hộp thư."
        };
    }


    public async Task<ForgotPasswordResponse> ResetPasswordWithOtpAsync(ResetPasswordWithOtpRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return new ForgotPasswordResponse { Success = false, Message = "Email không được để trống" };

        if (string.IsNullOrWhiteSpace(request.Otp))
            return new ForgotPasswordResponse { Success = false, Message = "Mã OTP không được để trống" };

        if (string.IsNullOrWhiteSpace(request.NewPassword))
            return new ForgotPasswordResponse { Success = false, Message = "Mật khẩu mới không được để trống" };

        if (request.NewPassword.Length < 6)
            return new ForgotPasswordResponse { Success = false, Message = "Mật khẩu mới phải có ít nhất 6 ký tự" };

        if (request.NewPassword.Length > 32)
            return new ForgotPasswordResponse { Success = false, Message = "Mật khẩu mới phải có tối đa 32 ký tự" };

        if (request.NewPassword != request.ConfirmPassword)
            return new ForgotPasswordResponse { Success = false, Message = "Mật khẩu mới và xác nhận mật khẩu không khớp" };

        var cleanEmail = request.Email.Trim().ToLower();
        var cacheKey = $"FORGOT_OTP_{cleanEmail}";

        if (!_memoryCache.TryGetValue(cacheKey, out string? cachedOtp))
        {
            return new ForgotPasswordResponse
            {
                Success = false,
                Message = "Mã OTP đã hết hạn hoặc không tồn tại. Vui lòng gửi lại yêu cầu mới."
            };
        }

        if (cachedOtp != request.Otp.Trim())
        {
            return new ForgotPasswordResponse { Success = false, Message = "Mã OTP không chính xác" };
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == cleanEmail);
        if (user == null || user.IsActive == false)
        {
            return new ForgotPasswordResponse { Success = false, Message = "Tài khoản không hợp lệ hoặc đã bị khóa" };
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _memoryCache.Remove(cacheKey);

        return new ForgotPasswordResponse
        {
            Success = true,
            Message = "Đặt lại mật khẩu thành công! Bạn có thể đăng nhập ngay bây giờ."
        };
    }
}
