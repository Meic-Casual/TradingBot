using Library.Bot;

namespace Library;

public class PriceTrailingCondition : ActionSkipConditionBase
{

    protected override bool SkipConditionMetSafe(IBotContext context)
    {
        if (context.PriceTrailing != null)
        {
            context.PriceTrailing.UpdateCurrentValue(context.LastRecordedPrice);
            return !context.PriceTrailing.IsTrailingConcluded;
        }
        else return false;
    }

}