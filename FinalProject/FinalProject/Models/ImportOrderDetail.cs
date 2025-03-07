using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class ImportOrderDetail
{
    public int Ioid { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public long? ImportPrice { get; set; }

    public virtual ImportOrder Io { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
