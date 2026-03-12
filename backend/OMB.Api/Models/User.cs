using System;
using System.Collections.Generic;

namespace OMB.Api.Models;

public partial class User
{
    public long Id { get; set; }

    public string Email { get; set; } = null!;

    public string DisplayNameFirst { get; set; } = null!;

    public string DisplayNameLast { get; set; } = null!;

    public long? DefaultLocationId { get; set; }

    public string? AuthProvider { get; set; }

    public string? AuthSubject { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Location? DefaultLocation { get; set; }

    public virtual ICollection<Order> OrderLockedByUsers { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderSubmittedByUsers { get; set; } = new List<Order>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
