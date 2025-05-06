using System;
using System.Collections.Generic;

namespace Models.Scaffolded;

public partial class User
{
    public int Id { get; set; }

    public string EmailHash { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<Bot> Bots { get; set; } = new List<Bot>();
}
