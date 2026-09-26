using System;
using System.Collections.Generic;

namespace StudioWeddingServer.Models;

public partial class ImageBanner
{
    public long ImageBannerId { get; set; }

    public long BannerId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string? MobileImageUrl { get; set; }

    public string? AltText { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Banner Banner { get; set; } = null!;
}
