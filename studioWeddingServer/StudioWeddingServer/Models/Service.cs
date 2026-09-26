using System;
using System.Collections.Generic;

namespace StudioWeddingServer.Models;

public partial class Service
{
    public long ServiceId { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int? DurationMinutes { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<BookingItem> BookingItems { get; set; } = new List<BookingItem>();

    public virtual ICollection<PackageService> PackageServices { get; set; } = new List<PackageService>();
}
