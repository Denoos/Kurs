using System;
using System.Collections.Generic;

namespace APIKurs.Models;

public partial class Need
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int PpeId { get; set; }

    public bool? IsActual { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Ppe Ppe { get; set; } = null!;
}
