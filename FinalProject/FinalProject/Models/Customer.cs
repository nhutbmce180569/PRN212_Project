using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public DateTime? Birthday { get; set; }

    public string Password { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? IsBlock { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
