using System;
using System.Collections.Generic;

namespace Models.Scaffolded;

public partial class Bot
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public decimal CurrentAllocation { get; set; }

    public decimal OverallAllowance { get; set; }

    public decimal MaxPriceBasePaddingPercent { get; set; }

    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
