using System;
using System.Collections.Generic;

namespace Common.Entity;

public partial class Order
{
    public Guid OrderId { get; set; }

    public Guid UserId { get; set; }

    public string Status { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdateAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User User { get; set; } = null!;
}
