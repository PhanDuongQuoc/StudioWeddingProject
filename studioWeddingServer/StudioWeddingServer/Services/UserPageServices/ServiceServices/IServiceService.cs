using StudioWeddingServer.DTOs.UserPageDTOs.ServiceDTOs;

namespace StudioWeddingServer.Services.UserPageServices.ServiceServices;

public interface IServiceService
{
    Task<ServiceListResponseDto> GetServicesAsync(string? search, int page = 1, int pageSize = 12);
    Task<ServiceDetailRespone> GetServiceDetailAsync(string slug, int otherPage = 1, int otherPageSize = 6);
}
