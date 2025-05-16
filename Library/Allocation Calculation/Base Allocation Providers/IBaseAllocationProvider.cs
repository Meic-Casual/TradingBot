using Library.Bot;

namespace Library;

public interface IBaseAllocationProvider
{

    decimal GetBaseAmount(IBotContext context);

}