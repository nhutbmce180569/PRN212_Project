using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class OrderStatus
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;
}
