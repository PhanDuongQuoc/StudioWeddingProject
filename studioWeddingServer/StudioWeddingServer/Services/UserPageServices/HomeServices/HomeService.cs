using Microsoft.EntityFrameworkCore;
using StudioWeddingServer.DTOs.UserPageDTOs.HomeDTOs;
using StudioWeddingServer.Models;
using static System.DateTime;

namespace StudioWeddingServer.Services.UserPageServices.HomeServices;

public class HomeService : IHomeService
{
    private readonly StudioWeddingDbContext _context;
    public HomeService(StudioWeddingDbContext context)
    {
        _context = context;
    }


    public async Task<HomeDto> GetHomePageDataAsync()
    {
        var now = DateTime.UtcNow;
        // Take list banners is active
        var banners = await _context.Banners.AsNoTracking()
        .Where(b => b.IsActive &&
                        (b.StartDate == null || b.StartDate <= now) &&
                        (b.EndDate == null || b.EndDate >= now))
                        .OrderBy(s => s.DisplayOrder)
                        .Select(s => new HomeBannerDto
                        {
                            BannerId = s.BannerId,
                            Title = s.Title ?? "",
                            Description = s.Description,
                            LinkUrl = s.LinkUrl,
                            DisplayOrder = s.DisplayOrder,
                            Images = s.ImageBanners
                                .Where(s => s.IsActive)
                                .OrderBy(s => s.DisplayOrder)
                                .Select(s => new HomeImageBannerDto
                                {
                                    ImageBannerId = s.ImageBannerId,
                                    ImageUrl = s.ImageUrl,
                                    MobileImageUrl = s.MobileImageUrl,
                                    AltText = s.AltText,
                                    DisplayOrder = s.DisplayOrder
                                })
                                .ToList()

                        }).ToListAsync();

        // Take featured albums list
        var featuredAlbums = await _context.Albums.AsNoTracking()
        .Include(s => s.Category)
        .Where(s => s.IsActive && s.IsFeatured)
        .OrderBy(s => s.DisplayOrder)
        .Take(8)
        .Select(s => new HomeAlbumDto
        {
            AlbumId = s.AlbumId,
            Title = s.Title ?? "",
            Slug = s.Slug ?? "",
            Description = s.Description,
            CoverImageUrl = s.CoverImageUrl,
            CategoryId = s.CategoryId,
            CategoryName = s.Category.Name,
            CategorySlug = s.Category.Slug,
            TotalPhotos = s.Photos.Count(p => p.IsActive)
        }).ToListAsync();

        // 3. Lấy danh sách Dịch vụ đang hoạt động
        var services = await _context.Services
            .AsNoTracking()
            .Where(s => s.IsActive)
            .OrderBy(s => s.ServiceId)
            .Take(6)
            .Select(s => new HomeServiceDto
            {
                ServiceId = s.ServiceId,
                Name = s.Name,
                Slug = s.Slug,
                Description = s.Description,
                Price = s.Price,
                DurationMinutes = s.DurationMinutes,
                ImageUrl = s.ImageUrl
            })
            .ToListAsync();
        // 4. Lấy các Gói dịch vụ combo (kèm tên các dịch vụ đi kèm)
        var packages = await _context.Packages
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Price)
            .Select(p => new HomePackageDto
            {
                PackageId = p.PackageId,
                Name = p.Name,
                Slug = p.Slug,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                IncludedServices = p.PackageServices
                    .Where(ps => ps.Service.IsActive)
                    .Select(ps => ps.Service.Name)
                    .ToList()
            })
            .ToListAsync();
        // 5. Lấy các Đánh giá đã được duyệt (Approved)
        var reviews = await _context.Reviews
            .AsNoTracking()
            .Where(r => r.IsApproved)
            .OrderByDescending(r => r.CreatedAt)
            .Take(6)
            .Select(r => new HomeReviewDto
            {
                ReviewId = r.ReviewId,
                CustomerName = r.Customer.FullName,
                Rating = r.Rating,
                Title = r.Title,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();
        // Gom toàn bộ vào HomeDto trả về
        return new HomeDto
        {
            Banners = banners,
            FeaturedAlbums = featuredAlbums,
            Services = services,
            Packages = packages,
            Reviews = reviews
        };

    }

}

