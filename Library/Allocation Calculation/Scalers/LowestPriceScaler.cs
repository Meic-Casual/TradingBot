using Library.Bot;

namespace Library;

public class LowestPriceScaler : AllocationScalerBase
{

    protected override decimal CalculateScaleSafe(IBotContext context)
    {
        if (MathUtils.AnyNonPositive(context.AveragePurchasePrice, context.LastRecordedPrice, context.LowestRecordedPrice)) return 0m;
        var low = context.LowestRecordedPrice;
        var currentToAverageValue = context.AveragePurchasePrice - context.LastRecordedPrice;
        var currentToMinValue = context.AveragePurchasePrice - low;
        return Math.Clamp(currentToAverageValue / currentToMinValue, 0m, 1m);
    }

}
