using System;
using System.Collections.Generic;

namespace CLientApp.Model;

public partial class Person
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public int PostId { get; set; }

    public int StatusId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual Ppe? Ppe { get; set; }

    public virtual Status Status { get; set; } = null!;

    public virtual string AllToString { get => $"{Name} {Surname} {Patronymic}"; }
}
