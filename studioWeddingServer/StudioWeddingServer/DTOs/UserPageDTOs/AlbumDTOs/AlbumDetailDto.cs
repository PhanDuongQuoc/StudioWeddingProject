namespace StudioWeddingServer.DTOs.UserPageDTOs.AlbumDTOs;

public class AlbumDetailDto
{
    public long AlbumId { get; set; }
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;
    public List<PhotoDto> Photos { get; set; } = [];
}

public class PhotoDto
{
    public long PhotoId { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string? ThumbnailUrl { get; set; }
    public string? Caption { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
