using StudioWeddingServer.DTOs.UserPageDTOs.AboutDTOs;

namespace StudioWeddingServer.Services.UserPageServices.AboutServices;

public interface IAboutService
{
    Task<AboutResponse> GetAboutAsync(int? aboutId = null);
}
