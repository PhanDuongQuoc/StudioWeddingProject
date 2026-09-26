using System;
using System.Collections.Generic;

namespace StudioWeddingServer.Models;

public partial class ContactRequest
{
    public long ContactRequestId { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Subject { get; set; }

    public string Message { get; set; } = null!;

    public string Status { get; set; } = null!;

    public Guid? HandledBy { get; set; }

    public DateTime? HandledAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? HandledByNavigation { get; set; }
}
