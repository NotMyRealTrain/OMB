using System;
using System.Collections.Generic;

namespace OMB.Api.Models;

public partial class Week
{
    public long Id { get; set; }

    public int IsoYear { get; set; }

    public int IsoWeek { get; set; }

    public DateOnly WeekStartDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
