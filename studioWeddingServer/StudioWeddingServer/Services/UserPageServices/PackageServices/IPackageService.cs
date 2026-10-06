using StudioWeddingServer.DTOs.UserPageDTOs.PackageDTOs;

namespace StudioWeddingServer.Services.UserPageServices.PackageServices;

public interface IPackageService
{
    Task<PackageDetailResponse> GetPackageDetailAsync(string slug, int orderPage = 1, int orderPageSize = 3);
}
