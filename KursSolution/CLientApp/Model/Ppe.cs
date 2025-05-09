using System;
using System.Collections.Generic;

namespace CLientApp.Model;

public partial class Ppe
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string InventoryNumber { get; set; } = null!;

    public int TypeId { get; set; }

    public int ConditionId { get; set; }

    public DateOnly DateGet { get; set; }

    public DateOnly DateEnd { get; set; }

    public virtual Condition Condition { get; set; } = null!;

    public virtual PpeType Type { get; set; } = null!;

    public virtual ICollection<Person> IdPeople { get; set; } = new List<Person>();
}
