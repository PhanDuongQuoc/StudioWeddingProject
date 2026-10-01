using Microsoft.EntityFrameworkCore;
using StudioWeddingServer.DTOs.UserPageDTOs.AuthDTOs;
using StudioWeddingServer.Models;

namespace StudioWeddingServer.Services.AuthServices.RegisterServices;

public class RegisterService : IRegisterService
{
    private readonly StudioWeddingDbContext _context;

    public RegisterService(StudioWeddingDbContext context)
    {
        _context = context;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return new RegisterResponse
            {
                Success = false,
                Message = "Vui lòng điền đầy đủ các thông tin bắt buộc!"
            };
        }

        string username = request.Username.Trim().ToLower();
        string email = request.Email.Trim().ToLower();
        string? phone = request.Phone?.Trim();
        string? fullName = request.FullName?.Trim();

        bool usernameExists = await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Username.ToLower() == username);

        if (usernameExists)
        {
            return new RegisterResponse
            {
                Success = false,
                Message = "Tên đăng nhập này đã được sử dụng!"
            };
        }

        // 3. Kiểm tra trùng Email
        bool emailExists = await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email.ToLower() == email);

        if (emailExists)
        {
            return new RegisterResponse
            {
                Success = false,
                Message = "Email này đã được đăng ký tài khoản!"
            };
        }

        // 4. Hash mật khẩu bằng BCrypt
        string encryptedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 5. Khởi tạo User mới
        var newUser = new User
        {
            UserId = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = encryptedPassword,
            FullName = !string.IsNullOrWhiteSpace(fullName) ? fullName : username,
            Phone = phone,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // 6. Gán Role mặc định (ví dụ: 'Customer' hoặc 'User')
        var defaultRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name.ToUpper() == "CUSTOMER" || r.Name.ToUpper() == "USER");

        if (defaultRole != null)
        {
            newUser.UserRoles.Add(new UserRole
            {
                UserId = newUser.UserId,
                RoleId = defaultRole.RoleId,
                AssignedAt = DateTime.UtcNow
            });
        }

        // 7. Lưu vào CSDL
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return new RegisterResponse
        {
            Success = true,
            Message = "Đăng ký tài khoản thành công!",
            User = new UserDto
            {
                UserId = newUser.UserId,
                Username = newUser.Username,
                Email = newUser.Email,
                FullName = newUser.FullName ?? string.Empty,
                CreatedAt = newUser.CreatedAt
            }
        };
    }
}
