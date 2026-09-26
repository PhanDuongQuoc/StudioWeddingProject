using System;
using System.Collections.Generic;

namespace StudioWeddingServer.Models;

public partial class UserRole
{
    public Guid UserId { get; set; }

    public long RoleId { get; set; }

    public DateTime AssignedAt { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
