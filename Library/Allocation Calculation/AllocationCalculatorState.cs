using Library.Bot;
using Library.Trailing;

namespace Library;

public class AllocationCalculatorState
{

    //public record ScalerRule(IAllocationScaler scaler, Func<IBotContext, bool> condition);
    internal bool IsEnabled(IAllocationScaler scaler)
    {
        return true;
    }

    internal decimal CalculateWeightedScale(IEnumerable<AllocationScaleEntry> normalizedScalesByScaler, bool registerAsLatestCalculationCycle = true)
    {
        //here would be calculation of the proper output value
        if (registerAsLatestCalculationCycle)
        {
            RegisterAsLatestCalculationCycle(normalizedScalesByScaler);
        }
        return default;
    }

    internal void RegisterAsLatestCalculationCycle(IEnumerable<AllocationScaleEntry> normalizedScalesByScaler)
    {
        //here would be scaler rules calculation
    }

}
