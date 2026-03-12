using System;
using System.Collections.Generic;
using OMB.Api.Enums;

namespace OMB.Api.Models;

public partial class Role
{
    public long Id { get; set; }

    public RoleName Name { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
