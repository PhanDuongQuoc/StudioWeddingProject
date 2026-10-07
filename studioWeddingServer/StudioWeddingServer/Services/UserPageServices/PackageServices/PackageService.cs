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

    public async Task<PackageListResponseDto> GetPackagesAsync(string? search, int page = 1, int pageSize = 12)
    {
        if (page < 1) page = 1;
        if (pageSize <= 0) pageSize = 12;

        var query = _context.Packages
            .AsNoTracking()
            .Include(p => p.PackageServices)
                .ThenInclude(ps => ps.Service)
            .Include(p => p.BookingItems)
            .Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(term) || (p.Description != null && p.Description.ToLower().Contains(term)));
        }

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = await query
            .OrderBy(p => p.Price)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PackageItemDto
            {
                PackageId = p.PackageId,
                Name = p.Name,
                Slug = p.Slug,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CountBooking = p.BookingItems.Count,
                IsPopular = p.BookingItems.Count >= 2 || p.Name.Contains("Cao Cấp") || p.Name.Contains("Premium"),
                IncludedServiceNames = p.PackageServices
                    .Where(ps => ps.Service != null && ps.Service.IsActive)
                    .Select(ps => ps.Service.Name)
                    .ToList(),
                IncludedServices = p.PackageServices
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
                    .ToList(),
                OriginalTotalPrice = p.PackageServices
                    .Where(ps => ps.Service != null && ps.Service.IsActive)
                    .Sum(ps => ps.Service.Price * (ps.Quantity > 0 ? ps.Quantity : 1)),
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        return new PackageListResponseDto
        {
            Items = items,
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            Success = true,
            Message = "Thành công"
        };
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

        // Query danh sách các gói cưới gợi ý khác (Phân trang)
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
