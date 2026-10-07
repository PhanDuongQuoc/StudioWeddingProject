namespace StudioWeddingServer.DTOs.UserPageDTOs.PackageDTOs;

public class PackageItemDto
{
    public long PackageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int CountBooking { get; set; }
    public decimal? OriginalTotalPrice { get; set; }
    public bool IsPopular { get; set; }
    public List<string> IncludedServiceNames { get; set; } = new();
    public List<PackageIncludedServiceDto> IncludedServices { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class PackageListResponseDto
{
    public List<PackageItemDto> Items { get; set; } = new();
    public int TotalItems { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalPages { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}
