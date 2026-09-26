using System;
using System.Collections.Generic;

namespace StudioWeddingServer.Models;

public partial class PortfolioCategory
{
    public long CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
}
