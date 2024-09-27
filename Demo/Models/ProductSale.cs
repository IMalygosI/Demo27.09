using System;
using System.Collections.Generic;

namespace Demo.Models;

public partial class ProductSale
{
    public int Id { get; set; }

    public string? TovarName { get; set; }

    public int Count { get; set; }

    public DateOnly DateOfSale { get; set; }

    public TimeOnly SalesTime { get; set; }

    public int? IdProduct { get; set; }

    public virtual Product? IdProductNavigation { get; set; }
}
