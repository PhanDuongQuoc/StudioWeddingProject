using StudioWeddingServer.DTOs.UserPageDTOs.PackageDTOs;

namespace StudioWeddingServer.Services.UserPageServices.PackageServices;

public interface IPackageService
{
    Task<PackageListResponseDto> GetPackagesAsync(string? search, int page = 1, int pageSize = 12);
    Task<PackageDetailResponse> GetPackageDetailAsync(string slug, int orderPage = 1, int orderPageSize = 3);
}
