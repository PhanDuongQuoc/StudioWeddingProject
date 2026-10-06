using Microsoft.EntityFrameworkCore;
using StudioWeddingServer.DTOs.UserPageDTOs.PackageDTOs;
using StudioWeddingServer.Models;

namespace StudioWeddingServer.Services.UserPageServices.PackageServices;

public class PackageService : IPackageService
{
    private readonly StudioWeddingDbContext _context;

    public PackageService(StudioWeddingDbContext context)
    {
        _context = context;
    }

    public async Task<PackageDetailResponse> GetPackageDetailAsync(string slug, int orderPage = 1, int orderPageSize = 3)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return new PackageDetailResponse
            {
                Success = false,
                Message = "Slug không được để trống"
            };
        }

        if (orderPage <= 0) orderPage = 1;
        if (orderPageSize <= 0) orderPageSize = 3;

        var package = await _context.Packages.AsNoTracking()
            .Include(p => p.PackageServices)
                .ThenInclude(ps => ps.Service)
            .Include(p => p.BookingItems)
            .FirstOrDefaultAsync(p => p.Slug == slug.ToLower().Trim() && p.IsActive);

        if (package == null)
        {
            return new PackageDetailResponse
            {
                Success = false,
                Message = "Không tìm thấy gói cưới này"
            };
        }

        var includedServices = package.PackageServices
            .Where(ps => ps.Service != null && ps.Service.IsActive)
            .Select(ps => new PackageIncludedServiceDto
            {
                ServiceId = ps.ServiceId,
                Name = ps.Service.Name,
                Slug = ps.Service.Slug,
                Description = ps.Service.Description,
                UnitPrice = ps.Service.Price,
                Quantity = ps.Quantity > 0 ? ps.Quantity : 1,
                DurationMinutes = ps.Service.DurationMinutes,
                ImageUrl = ps.Service.ImageUrl
            })
            .ToList();

        // 4. Query danh sách các gói cưới gợi ý khác (Phân trang)
        var otherQuery = _context.Packages.AsNoTracking()
            .Where(p => p.IsActive && p.PackageId != package.PackageId);

        var otherTotal = await otherQuery.CountAsync();
        var otherPackages = await otherQuery
            .OrderByDescending(p => p.CreatedAt)
            .Skip((orderPage - 1) * orderPageSize)
            .Take(orderPageSize)
            .Select(p => new RelatedPackageResponse
            {
                PackageId = p.PackageId,
                Name = p.Name,
                Slug = p.Slug,
                Price = p.Price,
                ImageUrl = p.ImageUrl
            })
            .ToListAsync();

        // 5. Trả về kết quả hoàn chỉnh
        return new PackageDetailResponse
        {
            PackageId = package.PackageId,
            Name = package.Name,
            Slug = package.Slug,
            Description = package.Description,
            Price = package.Price,
            ImageUrl = package.ImageUrl,
            CountBooking = package.BookingItems.Count,
            OriginalTotalPrice = includedServices.Sum(s => s.UnitPrice * s.Quantity),
            CreatedAt = package.CreatedAt,
            UpdatedAt = package.UpdatedAt,
            IncludedServices = includedServices,
            OtherPackages = otherPackages,
            OtherPage = orderPage,
            OtherPageSize = orderPageSize,
            OtherTotal = otherTotal,
            Success = true,
            Message = "Lấy thông tin gói cưới thành công"
        };
    }
}
