using Library.Bot;

namespace Library;

public class PriceToAverageScaler : AllocationScalerBase
{

    protected override decimal CalculateScaleSafe(IBotContext context)
    {
        var avg = context.AveragePurchasePrice;
        if (avg == 0) return 0;
        var ratio = Math.Clamp((avg - context.LastRecordedPrice) / avg, 0m, 1m);
        return ratio;
    }

}
