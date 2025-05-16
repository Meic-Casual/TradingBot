using Library.Bot;

namespace Library;

public interface IActionSkipCondition
{

    bool SkipConditionMet(IBotContext context);

}