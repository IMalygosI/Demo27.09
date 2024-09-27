using System;
using System.Collections.Generic;

namespace Demo.Models;

public partial class Image
{
    public int Id { get; set; }

    public string Picture { get; set; } = null!;

    public int IdProduct { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;
}
