using System;
using System.Collections.Generic;

namespace CLientApp.Model;

public partial class Role
{
    public int Id { get; set; }

    public string Ttle { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
