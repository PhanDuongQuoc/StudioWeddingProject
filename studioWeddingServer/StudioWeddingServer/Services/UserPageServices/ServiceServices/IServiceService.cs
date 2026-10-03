using StudioWeddingServer.DTOs.UserPageDTOs.ServiceDTOs;

namespace StudioWeddingServer.Services.UserPageServices.ServiceServices;

public interface IServiceService
{
    Task<ServiceDetailRespone> GetServiceDetailAsync(string slug, int otherPage = 1, int otherPageSize = 6);
}
