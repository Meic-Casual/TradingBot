using System;
using System.Collections.Generic;

namespace Models.Scaffolded;

public partial class Botstatesnapshot
{
    public int BotId { get; set; }

    public decimal RemainingAllowance { get; set; }

    public decimal AveragePurchasePrice { get; set; }

    public decimal LowestPurchasePrice { get; set; }

    public virtual Bot Bot { get; set; } = null!;
}
