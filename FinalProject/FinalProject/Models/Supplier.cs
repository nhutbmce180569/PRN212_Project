using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string? TaxId { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastModify { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool? IsActivate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<ImportOrder> ImportOrders { get; set; } = new List<ImportOrder>();
}
