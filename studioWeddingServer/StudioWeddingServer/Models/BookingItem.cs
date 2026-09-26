using System;
using System.Collections.Generic;

namespace StudioWeddingServer.Models;

public partial class BookingItem
{
    public long BookingItemId { get; set; }

    public long BookingId { get; set; }

    public long? ServiceId { get; set; }

    public long? PackageId { get; set; }

    public string ItemName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Package? Package { get; set; }

    public virtual Service? Service { get; set; }
}
