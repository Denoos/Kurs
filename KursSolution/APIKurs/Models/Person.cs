using System;
using System.Collections.Generic;

namespace APIKurs.Models;

public partial class Person
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public int PostId { get; set; }

    public int StatusId { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<Ppe> IdPpes { get; set; } = new List<Ppe>();
}
