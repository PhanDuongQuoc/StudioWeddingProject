namespace StudioWeddingServer.DTOs.UserPageDTOs.HomeDTOs;

/// <summary>
/// DTO tổng hợp dữ liệu cho toàn bộ Home Page
/// </summary>
public class HomeDto
{
    public List<HomeBannerDto> Banners { get; set; } = [];
    public List<HomeAlbumDto> FeaturedAlbums { get; set; } = [];
    public List<HomeServiceDto> Services { get; set; } = [];
    public List<HomePackageDto> Packages { get; set; } = [];
    public List<HomeReviewDto> Reviews { get; set; } = [];
}

/// <summary>
/// DTO Banner hiển thị trên Slider / Hero section
/// </summary>
public class HomeBannerDto
{
    public long BannerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LinkUrl { get; set; }
    public int DisplayOrder { get; set; }
    public List<HomeImageBannerDto> Images { get; set; } = [];
}

/// <summary>
/// DTO hình ảnh của Banner (hỗ trợ responsive mobile / desktop)
/// </summary>
public class HomeImageBannerDto
{
    public long ImageBannerId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? MobileImageUrl { get; set; }
    public string? AltText { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>
/// DTO Album nổi bật (Featured Albums / Portfolio)
/// </summary>
public class HomeAlbumDto
{
    public long AlbumId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public long CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? CategorySlug { get; set; }
    public int TotalPhotos { get; set; }
}

/// <summary>
/// DTO Dịch vụ đơn lẻ (Chụp ảnh cưới, Makeup, Thuê váy...)
/// </summary>
public class HomeServiceDto
{
    public long ServiceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int? DurationMinutes { get; set; }
    public string? ImageUrl { get; set; }
}

/// <summary>
/// DTO Gói dịch vụ combo / trọn gói
/// </summary>
public class HomePackageDto
{
    public long PackageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public List<string> IncludedServices { get; set; } = []; // Danh sách tên các dịch vụ đi kèm trong gói
}

/// <summary>
/// DTO Đánh giá & Phản hồi từ khách hàng (Testimonials)
/// </summary>
public class HomeReviewDto
{
    public long ReviewId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public short Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}
