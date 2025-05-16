using Library.Bot;

namespace Library;

public abstract class ActionSkipConditionBase : IActionSkipCondition
{

    public bool SkipConditionMet(IBotContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var result = SkipConditionMetSafe(context);
        Console.WriteLine(this.GetType().Name + " - " + result);
        return result;// SkipConditionMetSafe(context);
    }

    protected abstract bool SkipConditionMetSafe(IBotContext context);

}