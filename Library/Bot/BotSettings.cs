﻿using Library.Trailing;

namespace Library.Bot
{

    /// <param name="minProfitRatePercent">Profit threshold in percent (e.g. 1.5 means 1.5%).</param>
    public class BotSettings(
        decimal baseStepFunds,
        decimal minProfitRatePercent = 0m,
        ValueTrailingSettings? trailingSetting = null) : IHost<ValueTrailingSettings>
    {

        public decimal BaseStepFunds { get; } = baseStepFunds;
        public decimal MinProfitRatePercent { get; } = minProfitRatePercent;
        public ValueTrailingSettings? TrailingSetting { get; } = trailingSetting;
        public decimal MinDeviationFromAveragePercent { get; init; } = 0m;

        ValueTrailingSettings? IHost<ValueTrailingSettings>.Value => TrailingSetting;

    }

}