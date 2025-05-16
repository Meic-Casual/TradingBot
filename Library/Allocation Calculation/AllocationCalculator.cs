using Library.Bot;

namespace Library;

public class AllocationCalculator : IAllocationCalculator
{

    private readonly IBaseAllocationProvider baseProvider;
    private readonly IReadOnlyList<IAllocationScaler> scalers;

    public AllocationCalculator(IBaseAllocationProvider baseProvider, params IAllocationScaler[] scalers)
    {
        this.baseProvider = baseProvider;
        this.scalers = scalers.ToList();
    }

    public decimal GetAllocation(IBotContext context)
    {
        var amount = baseProvider.GetBaseAmount(context);
        foreach (var scaler in scalers)
        {
            amount = scaler.Scale(amount, context);
        }
        return amount;
    }

}
