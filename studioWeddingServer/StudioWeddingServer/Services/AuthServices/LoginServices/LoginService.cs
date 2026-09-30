using Microsoft.EntityFrameworkCore;
using StudioWeddingServer.DTOs.UserPageDTOs.AuthDTOs;
using StudioWeddingServer.Models;

namespace StudioWeddingServer.Services.AuthServices.LoginServices;

public class LoginService : ILoginService
{
    private readonly StudioWeddingDbContext _context;
    private readonly JwtService _jwtService;
    public LoginService(StudioWeddingDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        string email = request.Email?.Trim().ToLower() ?? string.Empty;
        string password = request.Password;
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return new LoginResponse
            {
                Success = true,
                Message = "Vui lòng nhập tên đăng nhập và mật khẩu"
            };
        }

        var user = await _context.Users.AsNoTracking().Include(u => u.UserRoles)
        .ThenInclude(u => u.Role)
        .ThenInclude(u => u.RolePermissions)
        .ThenInclude(u => u.Permission)
        .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Tên đăng nhập không tồn tại"
            };
        }
        if (!user.IsActive)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Tài khoản của bạn đã bị khóa hoặc chưa được kích hoạt!"
            };

        }
        var role = user.UserRoles.Where(s => s.Role != null)
        .Select(u => u.Role!.Name).Distinct().ToList();

        var permission = user.UserRoles.Where(s => s.Role != null)
        .SelectMany(p => p.Role.RolePermissions).Where(p => p.Permission != null)
        .Select(p => p.Permission!.Name).Distinct().ToList();
        bool isVerified = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!isVerified)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Mật khẩu không chính xác!"
            };
        }
        var accessToken = _jwtService.GenerateToken(user.UserId, user.Email, role);
        var result = new LoginResponse
        {
            Success = true,
            Message = "Đăng nhập thành công!",
            AccessToken = accessToken,
            TokenType = "Bearer",
            User = new UserProfileDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName
            },
            Roles = role,
            Permissions = permission,
        };

        return result;
    }


}
