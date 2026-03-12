using System;
using System.Collections.Generic;

namespace OMB.Api.Models;

public partial class Location
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Code { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<OrderResidentDayEntry> OrderResidentDayEntries { get; set; } = new List<OrderResidentDayEntry>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Resident> Residents { get; set; } = new List<Resident>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<User> UsersNavigation { get; set; } = new List<User>();
}
