using Library.Bot;
using Library.Indicators;

namespace Library;

public class DipScaler : AllocationScalerBase
{

    readonly RSI rsi = new();

    protected override decimal CalculateScaleSafe(IBotContext context)
    {
        return ((decimal)MathUtils.InverseLerp(0f, 100f, rsi.CalculateValue(context.RecordedPrices)));
        return default;
    }

}