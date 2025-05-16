using Library.Bot;
using Library.Indicators;

namespace Library;

public class OversoldScaler : AllocationScalerBase
{

    readonly RSI rsi = new RSI();

    protected override decimal CalculateScaleSafe(IBotContext context)
    {
        throw new NotImplementedException();
    }

}