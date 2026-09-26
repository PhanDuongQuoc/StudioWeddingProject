using System;
using System.Collections.Generic;

namespace StudioWeddingServer.Models;

public partial class Payment
{
    public long PaymentId { get; set; }

    public long BookingId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public string? TransactionCode { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Booking Booking { get; set; } = null!;
}
