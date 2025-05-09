namespace Models.Scaffolded;

public partial class Purchase
{

    public Purchase() { }

    public Purchase(decimal price, decimal cost)
    {
        Price = price;
        Cost = cost;
        Quantity = cost / price;
        PurchasedAt = DateTime.UtcNow;
    }

}
