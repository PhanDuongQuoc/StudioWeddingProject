using System;
using System.Collections.Generic;

namespace StudioWeddingServer.Models;

public partial class Review
{
    public long ReviewId { get; set; }

    public long CustomerId { get; set; }

    public long BookingId { get; set; }

    public short Rating { get; set; }

    public string? Title { get; set; }

    public string? Comment { get; set; }

    public bool IsApproved { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
