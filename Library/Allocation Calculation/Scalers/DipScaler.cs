using Library.Bot;
using Library.Indicators.Tools;

namespace Library;

public class DipScaler : AllocationScalerBase
{

    public const decimal DipDetectedNormalizedScale = 1m;
    public const decimal DipNotDetectedNormalizedScale = 0m;

    readonly RSIBasedDipDetector dipDetector;

    public DipScaler(RSIBasedDipDetector dipDetector) : base()
    {
        this.dipDetector = dipDetector;
    }

    protected override decimal CalculateScaleSafe(IBotContext context)
    {
        return dipDetector.IsDipDetected(context.RecordedPrices) ? DipDetectedNormalizedScale : DipNotDetectedNormalizedScale;
    }

}