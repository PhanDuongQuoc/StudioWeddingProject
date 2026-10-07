using Microsoft.EntityFrameworkCore;
using StudioWeddingServer.DTOs.UserPageDTOs.AlbumDTOs;
using StudioWeddingServer.Models;

namespace StudioWeddingServer.Services.UserPageServices.AlbumServices;

public class AlbumService : IAlbumService
{
    private readonly StudioWeddingDbContext _context;

    public AlbumService(StudioWeddingDbContext context)
    {
        _context = context;
    }

    public async Task<AlbumListResponseDto> GetAlbumsAsync(string? categorySlug, string? search, int page = 1, int pageSize = 9)
    {
        if (page < 1) page = 1;
        if (pageSize <= 0) pageSize = 9;

        var query = _context.Albums
            .AsNoTracking()
            .Where(a => a.IsActive);

        if (!string.IsNullOrWhiteSpace(categorySlug) && !categorySlug.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            var normalizedCatSlug = categorySlug.Trim().ToLower();
            query = query.Where(a => a.Category.Slug == normalizedCatSlug);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(a => a.Title.ToLower().Contains(term) || (a.Description != null && a.Description.ToLower().Contains(term)));
        }

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = await query
            .OrderByDescending(a => a.IsFeatured)
            .ThenBy(a => a.DisplayOrder)
            .ThenByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new AlbumItemDto
            {
                AlbumId = a.AlbumId,
                Title = a.Title,
                Slug = a.Slug,
                Description = a.Description,
                CoverImageUrl = a.CoverImageUrl,
                CategoryId = a.CategoryId,
                CategoryName = a.Category.Name,
                CategorySlug = a.Category.Slug,
                TotalPhotos = a.Photos.Count(p => p.IsActive),
                IsFeatured = a.IsFeatured,
                DisplayOrder = a.DisplayOrder,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();

        var categories = await _context.PortfolioCategories
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new CategoryFilterDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Slug = c.Slug,
                AlbumCount = c.Albums.Count(a => a.IsActive)
            })
            .ToListAsync();

        return new AlbumListResponseDto
        {
            Items = items,
            Categories = categories,
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            Success = true,
            Message = "Thành công"
        };
    }

    public async Task<AlbumDetailDto?> GetAlbumDetailBySlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return null;
        }

        var normalizedSlug = slug.Trim().ToLower();

        var album = await _context.Albums
            .AsNoTracking()
            .Where(a => a.Slug == normalizedSlug && a.IsActive)
            .Select(a => new AlbumDetailDto
            {
                AlbumId = a.AlbumId,
                Title = a.Title,
                Slug = a.Slug,
                Description = a.Description,
                CoverImageUrl = a.CoverImageUrl,
                IsFeatured = a.IsFeatured,
                IsActive = a.IsActive,
                DisplayOrder = a.DisplayOrder,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                CategoryName = a.Category.Name,
                CategorySlug = a.Category.Slug,
                Photos = a.Photos
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.DisplayOrder)
                    .Select(p => new PhotoDto
                    {
                        PhotoId = p.PhotoId,
                        ImageUrl = p.ImageUrl,
                        ThumbnailUrl = p.ThumbnailUrl,
                        Caption = p.Caption,
                        DisplayOrder = p.DisplayOrder,
                        IsActive = p.IsActive,
                        CreatedAt = p.CreatedAt,
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        return album;
    }
}
