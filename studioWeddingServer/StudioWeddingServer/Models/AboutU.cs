using System;
using System.Collections.Generic;

namespace StudioWeddingServer.Models;

public partial class AboutU
{
    public long AboutId { get; set; }

    public string HeroTitle { get; set; } = null!;

    public string? HeroSubtitle { get; set; }

    public string? HeroImageUrl { get; set; }

    public string StoryTitle { get; set; } = null!;

    public string StoryContent { get; set; } = null!;

    public string? StoryImageUrl { get; set; }

    public string? FounderName { get; set; }

    public string? FounderQuote { get; set; }

    public int? YearsExperience { get; set; }

    public int? HappyCouples { get; set; }

    public string? SatisfactionRate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AboutTimeline> AboutTimelines { get; set; } = new List<AboutTimeline>();
}
