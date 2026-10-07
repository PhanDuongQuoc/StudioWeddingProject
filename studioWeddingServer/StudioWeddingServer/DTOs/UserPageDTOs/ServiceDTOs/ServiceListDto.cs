namespace StudioWeddingServer.DTOs.UserPageDTOs.ServiceDTOs;

public class ServiceItemDto
{
    public long ServiceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int? DurationMinutes { get; set; }
    public string? ImageUrl { get; set; }
    public int CountBooking { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ServiceListResponseDto
{
    public List<ServiceItemDto> Items { get; set; } = new();
    public int TotalItems { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalPages { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}
