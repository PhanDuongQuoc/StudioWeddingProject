using System;
using System.Collections.Generic;

namespace StudioWeddingServer.Models;

public partial class Permission
{
    public long PermissionId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
