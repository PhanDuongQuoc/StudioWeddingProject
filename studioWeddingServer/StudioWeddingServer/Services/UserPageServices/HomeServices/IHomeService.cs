using StudioWeddingServer.DTOs.UserPageDTOs.HomeDTOs;

namespace StudioWeddingServer.Services.UserPageServices.HomeServices;

public interface IHomeService
{
    Task<HomeDto> GetHomePageDataAsync();
}
