using System;
using System.Collections.Generic;

namespace Common.Entity;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
