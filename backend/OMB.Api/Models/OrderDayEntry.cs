using System;
using System.Collections.Generic;
using OMB.Api.Enums;

namespace OMB.Api.Models;

public partial class OrderDayEntry
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public DateOnly ServiceDate { get; set; }

    public int ExtraMeals { get; set; }

    public int ExtraVegMeals { get; set; }

    public string NotesDay { get; set; } = null!;

    public BirthdayMeal BirthdayMeal { get; set; }

    public virtual Order Order { get; set; } = null!;
}
