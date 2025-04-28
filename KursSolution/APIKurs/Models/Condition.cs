using System;
using System.Collections.Generic;

namespace APIKurs.Models;

public partial class Condition
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Ppe> Ppes { get; set; } = new List<Ppe>();
}
