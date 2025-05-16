using Library.Bot;

namespace Library;

public class FixedBaseProvider : IBaseAllocationProvider
{

    private readonly decimal fixedAmount;

    public FixedBaseProvider(decimal amount) => fixedAmount = amount;

    public decimal GetBaseAmount(IBotContext _) => fixedAmount;

}
