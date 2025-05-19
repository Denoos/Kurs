using System;
using System.Collections.Generic;

namespace APIKurs.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Token { get; set; }

    public int IdRole { get; set; }

    public sbyte IsDeleted { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
