using Library.Bot;

namespace Library;

public class AllocationCalculator : IAllocationCalculator
{

    private readonly IBaseAllocationProvider baseProvider;
    private readonly IReadOnlyList<IAllocationScaler> scalers;

    public AllocationCalculator(IBaseAllocationProvider baseProvider, params IAllocationScaler[] scalers)
    {
        ArgumentNullException.ThrowIfNull(baseProvider);
        this.baseProvider = baseProvider;
        this.scalers = scalers.ToList();
    }

    public decimal GetAllocation(IBotContext context)
    {
        var amount = baseProvider.GetBaseAmount(context);
        var calculatorState = context.AllocationCalculatorState;
        var normalizedScalesByScaler = scalers.Where(calculatorState.IsEnabled).Select(s => new AllocationScaleEntry(s, s.CalculateScale(context)));
        return amount * calculatorState.CalculateWeightedScale(normalizedScalesByScaler, true);
    }

}
