using System;
using System.Collections.Generic;
using OMB.Api.Enums;


namespace OMB.Api.Models;

public partial class Order
{
    public long Id { get; set; }

    public long LocationId { get; set; }

    public long WeekId { get; set; }

    public string? GroupLeaderName { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public long? SubmittedByUserId { get; set; }

    public DateTime? LockedAt { get; set; }

    public long? LockedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public OrderStatus Status { get; set; }

    public virtual Location Location { get; set; } = null!;

    public virtual User? LockedByUser { get; set; }

    public virtual ICollection<OrderDayEntry> OrderDayEntries { get; set; } = new List<OrderDayEntry>();

    public virtual ICollection<OrderResidentDayEntry> OrderResidentDayEntries { get; set; } = new List<OrderResidentDayEntry>();

    public virtual User? SubmittedByUser { get; set; }

    public virtual Week Week { get; set; } = null!;

}
