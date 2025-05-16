using Library.Bot;

namespace Library;

public class RemainingFundsScaler : AllocationScalerBase
{

    protected override decimal CalculateScaleSafe(IBotContext context)
    {
        var weight = context.RemainingAllowance / context.OverallAllowance;
        return Math.Clamp(weight, 0m, 1m);
    }

}