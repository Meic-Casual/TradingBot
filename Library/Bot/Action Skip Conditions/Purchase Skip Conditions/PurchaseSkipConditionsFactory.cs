using Library.Bot;

namespace Library;

public static class PurchaseConditionFactory
{

    public static IEnumerable<IActionSkipCondition> FromSettings(BotSettings settings)
    {
        var conditions = new List<IActionSkipCondition>();

        if (settings.MinDeviationFromAveragePercent > 0)
            conditions.Add(new DeviationFromAverageCondition(settings.MinDeviationFromAveragePercent));

        return conditions;
    }

}
