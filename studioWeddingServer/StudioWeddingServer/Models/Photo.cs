using System;
using System.Collections.Generic;

namespace StudioWeddingServer.Models;

public partial class Photo
{
    public long PhotoId { get; set; }

    public long AlbumId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string? ThumbnailUrl { get; set; }

    public string? Caption { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Album Album { get; set; } = null!;
}
