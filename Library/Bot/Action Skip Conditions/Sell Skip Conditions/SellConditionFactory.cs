using Library.Bot;

namespace Library;

public static class SellConditionFactory
{

    public static IEnumerable<IActionSkipCondition> FromSettings(BotSettings settings)
    {
        var conditions = new List<IActionSkipCondition>();

        if (settings.MinProfitRatePercent > 0)
            conditions.Add(new ProfitRateCondition(settings.MinProfitRatePercent));

        if (settings.TrailingSetting != null)
            conditions.Add(new PriceTrailingCondition());

        return conditions;
    }

}
