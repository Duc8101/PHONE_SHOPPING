using System;
using System.Collections.Generic;

namespace DataAccess.Entity;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
