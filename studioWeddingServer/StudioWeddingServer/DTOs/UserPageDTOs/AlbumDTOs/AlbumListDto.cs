namespace StudioWeddingServer.DTOs.UserPageDTOs.AlbumDTOs;

public class AlbumItemDto
{
    public long AlbumId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public long CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;
    public int TotalPhotos { get; set; }
    public bool IsFeatured { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CategoryFilterDto
{
    public long CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int AlbumCount { get; set; }
}

public class AlbumListResponseDto
{
    public List<AlbumItemDto> Items { get; set; } = new();
    public List<CategoryFilterDto> Categories { get; set; } = new();
    public int TotalItems { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 9;
    public int TotalPages { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}
