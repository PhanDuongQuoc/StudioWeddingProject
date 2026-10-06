namespace StudioWeddingServer.DTOs.UserPageDTOs.PackageDTOs;

/// <summary>
/// DTO trả về toàn bộ thông tin chi tiết của 1 Gói cưới
/// </summary>
public class PackageDetailResponse
{
    public long PackageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }

    // Thống kê độ tin chọn (tính từ bảng BookingItems)
    public int CountBooking { get; set; } = 0;

    // Tổng giá gốc nếu mua lẻ các dịch vụ (để hiển thị so sánh: Tiết kiệm được bao nhiêu khi chọn gói)
    public decimal? OriginalTotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;

    // 1. Danh sách các Dịch vụ & Đặc quyền ĐÃ BAO GỒM trong Gói cưới này
    public List<PackageIncludedServiceDto> IncludedServices { get; set; } = new();

    // 2. Danh sách các Gói cưới gợi ý khác (Phục vụ phân trang mini ở chân trang)
    public List<RelatedPackageResponse> OtherPackages { get; set; } = new();
    public int OtherPage { get; set; } = 1;
    public int OtherPageSize { get; set; } = 3;
    public int OtherTotal { get; set; } = 0;
}

/// <summary>
/// Chi tiết từng dịch vụ con nằm trong Gói cưới (Lấy từ bảng PackageService & Service)
/// </summary>
public class PackageIncludedServiceDto
{
    public long ServiceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; } = 1; // Số lượng (ví dụ: 2 váy cưới, 1 album photobook, 2 lần makeup)
    public int? DurationMinutes { get; set; }
    public string? ImageUrl { get; set; }
}

/// <summary>
/// Gói cưới gợi ý khác hiển thị ở chân trang
/// </summary>
public class RelatedPackageResponse
{
    public long PackageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
}
