using StudioWeddingServer.DTOs.UserPageDTOs.AuthDTOs;

namespace StudioWeddingServer.Services.AuthServices.RegisterServices;

public interface IRegisterService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
}
