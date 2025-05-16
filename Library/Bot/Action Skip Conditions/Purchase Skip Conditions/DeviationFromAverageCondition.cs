using Library.Bot;

namespace Library;

public class DeviationFromAverageCondition(decimal minDeviationRatePercent) : ActionSkipConditionBase
{

    readonly decimal minDeviationRatePercent = minDeviationRatePercent;

    protected override bool SkipConditionMetSafe(IBotContext context)
    {
        var avg = context.AveragePurchasePrice;
        if (avg == 0) return false;
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"{(Math.Abs(avg - context.LastRecordedPrice) / avg):F4} vs {minDeviationRatePercent} from {avg} and {context.LastRecordedPrice} ({avg - context.LastRecordedPrice})");
        Console.ResetColor();
        return Math.Abs(avg - context.LastRecordedPrice) / avg < minDeviationRatePercent;
    }

}