using System;
using System.Collections.Generic;

namespace StudioWeddingServer.Models;

public partial class AboutTimeline
{
    public long TimelineId { get; set; }

    public long AboutId { get; set; }

    public string Year { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? DisplayOrder { get; set; }

    public virtual AboutU About { get; set; } = null!;
}
