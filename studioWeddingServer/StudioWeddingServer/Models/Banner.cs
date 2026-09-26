using System;
using System.Collections.Generic;

namespace StudioWeddingServer.Models;

public partial class Banner
{
    public long BannerId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? LinkUrl { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<ImageBanner> ImageBanners { get; set; } = new List<ImageBanner>();
}
