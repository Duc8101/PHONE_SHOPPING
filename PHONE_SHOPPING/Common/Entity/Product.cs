using System;
using System.Collections.Generic;

namespace Common.Entity;

public partial class Product
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string Image { get; set; } = null!;

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
