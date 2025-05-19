using System;
using System.Collections.Generic;

namespace CLientApp.Model;

public partial class Status
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
