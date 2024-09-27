﻿using System;
using System.Collections.Generic;

namespace Demo.Models;

public partial class Manufacturer
{
    public int Id { get; set; }

    public string ManufacturerName { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
