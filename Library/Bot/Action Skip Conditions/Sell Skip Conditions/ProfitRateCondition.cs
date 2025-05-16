using Library.Bot;

namespace Library;

public class ProfitRateCondition(decimal minProfitRatePercent) : ActionSkipConditionBase
{

    readonly decimal minProfitRatePercent = minProfitRatePercent;

    protected override bool SkipConditionMetSafe(IBotContext context)
    {
        var profitRate = (context.LastRecordedPrice - context.AveragePurchasePrice) / context.AveragePurchasePrice * 100m;
        return profitRate < minProfitRatePercent;
    }

}