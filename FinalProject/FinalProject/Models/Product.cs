using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int? BrandId { get; set; }

    public int? CategoryId { get; set; }

    public string? Model { get; set; }

    public string? FullName { get; set; }

    public string? Description { get; set; }

    public bool? IsDeleted { get; set; }

    public long? Price { get; set; }

    public int? Stock { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<ImportOrderDetail> ImportOrderDetails { get; set; } = new List<ImportOrderDetail>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
