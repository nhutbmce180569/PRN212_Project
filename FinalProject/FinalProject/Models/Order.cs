using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public DateTime OrderedDate { get; set; }

    public DateTime? DeliveredDate { get; set; }

    public int? Status { get; set; }

    public long? TotalAmount { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
