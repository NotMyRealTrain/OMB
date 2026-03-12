using System;
using System.Collections.Generic;

namespace OMB.Api.Models;

public partial class OrderResidentDayEntry
{
    public long OrderId { get; set; }

    public long LocationId { get; set; }

    public long ResidentId { get; set; }

    public DateOnly ServiceDate { get; set; }

    public bool Present { get; set; }

    public virtual Location Location { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Resident Resident { get; set; } = null!;
}
