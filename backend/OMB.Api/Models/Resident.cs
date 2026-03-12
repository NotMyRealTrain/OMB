using System;
using System.Collections.Generic;

namespace OMB.Api.Models;

public partial class Resident
{
    public long Id { get; set; }

    public long LocationId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? IddsiLevel { get; set; }

    public bool IsVegetarian { get; set; }

    public string AllergenNotes { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<OrderResidentDayEntry> OrderResidentDayEntries { get; set; } = new List<OrderResidentDayEntry>();
}
