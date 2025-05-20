using Library.Bot;
using Library.Indicators;

namespace Library;

public class OversoldScaler : AllocationScalerBase
{

    readonly RSI rsi = new RSI();
    readonly float rangeMinValue = 0f, rangeMaxValue = 100f;

    public OversoldScaler() { }

    /// <summary>
    /// The full range is 0 - 100.
    /// </summary>
    public OversoldScaler(float rangeMaxValue, float rangeMinValue = 0f)
    {
        this.rangeMaxValue = rangeMaxValue;
        this.rangeMinValue = rangeMinValue;
        if (rangeMinValue > rangeMaxValue)
        {
            throw new ArgumentException("The rangeMinValue should be lower than rangeMaxValue.");
        }
    }

    protected override decimal CalculateScaleSafe(IBotContext context)
    {
        Console.WriteLine($" ---- RSI: {rsi.CalculateValue(context.RecordedPrices)} for {context.LastRecordedPrice}");
        return ((decimal)MathUtils.InverseLerp(rangeMinValue, rangeMaxValue, rsi.CalculateValue(context.RecordedPrices)));
    }

}