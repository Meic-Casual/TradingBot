using System;
using System.Collections.Generic;

namespace Models.Scaffolded;

public partial class Purchase
{
    public int Id { get; set; }

    public int BotId { get; set; }

    public decimal Quantity { get; set; }

    public decimal Cost { get; set; }

    public decimal Price { get; set; }

    public DateTime PurchasedAt { get; set; }

    public virtual Bot Bot { get; set; } = null!;
}
