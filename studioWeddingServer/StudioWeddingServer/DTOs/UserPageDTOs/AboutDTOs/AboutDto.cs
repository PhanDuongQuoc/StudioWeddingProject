namespace StudioWeddingServer.DTOs.UserPageDTOs.AboutDTOs;

/// <summary>
/// DTO trả về toàn bộ dữ liệu hiển thị trên trang Giới thiệu (About Us)
/// </summary>
public class AboutResponse
{
    public long AboutId { get; set; }

    // 1. Phân đoạn Hero đầu trang
    public string HeroTitle { get; set; } = string.Empty;
    public string? HeroSubtitle { get; set; }
    public string? HeroImageUrl { get; set; }

    // 2. Phân đoạn Câu chuyện thương hiệu (Brand Story)
    public string StoryTitle { get; set; } = string.Empty;
    public string StoryContent { get; set; } = string.Empty;
    public string? StoryImageUrl { get; set; }

    // 3. Thông tin người sáng lập / Master Artist
    public string? FounderName { get; set; }
    public string? FounderQuote { get; set; }

    // 4. Các con số ấn tượng (Milestone Counters)
    public int YearsExperience { get; set; } = 28;
    public int HappyCouples { get; set; } = 3000;
    public string? SatisfactionRate { get; set; } = "99.8%";

    // 5. Danh sách các cột mốc lịch sử phát triển (Timeline 1998 -> 2026)
    public List<AboutTimelineDto> Timelines { get; set; } = new();

    // 6. Trạng thái phản hồi
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// DTO cho từng cột mốc lịch sử trong hành trình 28 năm
/// </summary>
public class AboutTimelineDto
{
    public long TimelineId { get; set; }
    public string Year { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; } = 0;
}
