using Library.Trailing;

namespace Library.Bot
{

    public interface IBotSettings
    {

        decimal BaseStepFunds { get; }
        decimal MinProfitRatePercent { get; }
        ValueTrailingSettings? TrailingSetting { get; }
        AllocationCalculatorSettings? AllocationCalculatorSettings { get; }
        decimal MinDeviationFromAveragePercent { get; }

    }

}