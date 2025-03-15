using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public DateTime? Birthday { get; set; }

    public string Password { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Status { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<ImportOrder> ImportOrders { get; set; } = new List<ImportOrder>();

    public virtual Role? Role { get; set; }
}
