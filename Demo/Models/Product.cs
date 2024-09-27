using Avalonia.Media.Imaging;
using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace Demo.Models;

public partial class Product
{
    public int Id { get; set; }

    public string TovarName { get; set; } = null!;

    public decimal Cost { get; set; }

    public string? MainImagePath { get; set; }

    public int IdActivity { get; set; }

    public int? IdManufacturer { get; set; }

    public string? Description { get; set; }

    public virtual Activity IdActivityNavigation { get; set; } = null!;

    public virtual Manufacturer? IdManufacturerNavigation { get; set; }

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<ProductSale> ProductSales { get; set; } = new List<ProductSale>();

    public Bitmap Pictures => MainImagePath != null ? new Bitmap($@"Assets\\Товары школы\\{MainImagePath}") : null!;

    public string Activity => IdActivity == 1 ? "Активный" : "Неактивный";

    public SolidColorBrush SColor => Activity == "Активный" ? new SolidColorBrush(Color.Parse("#e7fabf")) : new SolidColorBrush(Color.Parse("Gray"));
}
