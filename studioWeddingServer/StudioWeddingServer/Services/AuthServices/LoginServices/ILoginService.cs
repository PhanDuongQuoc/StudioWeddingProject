using StudioWeddingServer.DTOs.UserPageDTOs.AuthDTOs;

namespace StudioWeddingServer.Services.AuthServices.LoginServices;

public interface ILoginService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}
