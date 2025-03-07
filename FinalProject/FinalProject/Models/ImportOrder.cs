using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class ImportOrder
{
    public int Ioid { get; set; }

    public int? EmployeeId { get; set; }

    public int? SupplierId { get; set; }

    public DateTime? ImportDate { get; set; }

    public long? TotalCost { get; set; }

    public bool? Completed { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<ImportOrderDetail> ImportOrderDetails { get; set; } = new List<ImportOrderDetail>();

    public virtual Supplier? Supplier { get; set; }
}
