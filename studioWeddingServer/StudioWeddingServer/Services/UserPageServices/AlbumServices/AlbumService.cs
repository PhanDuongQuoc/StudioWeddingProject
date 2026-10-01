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
