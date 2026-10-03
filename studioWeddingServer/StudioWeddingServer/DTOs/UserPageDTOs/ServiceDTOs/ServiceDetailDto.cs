using System.Dynamic;

namespace StudioWeddingServer.DTOs.UserPageDTOs.ServiceDTOs;

public class ServiceDetailRespone
{
    public long ServiceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int? DurationMinutes { get; set; }
    public string? ImageUrl { get; set; }
    public int CountBooking { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;

    // Dịch vụ gợi ý khác
    public List<RelatedServiceRespone> OtherServices { get; set; } = new();
    public int OtherPage { get; set; } = 1;
    public int OtherPageSize { get; set; } = 6;
    public int OtherTotal { get; set; } = 0;
}

public class RelatedServiceRespone
{
    public long ServiceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
}
