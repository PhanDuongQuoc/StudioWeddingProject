using System;
using System.Collections.Generic;

namespace StudioWeddingServer.Models;

public partial class Booking
{
    public long BookingId { get; set; }

    public long CustomerId { get; set; }

    public string BookingCode { get; set; } = null!;

    public DateTime BookingDate { get; set; }

    public DateOnly? EventDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal Subtotal { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal DepositAmount { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<BookingItem> BookingItems { get; set; } = new List<BookingItem>();

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Review? Review { get; set; }
}
