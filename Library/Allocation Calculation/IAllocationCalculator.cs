using Library.Bot;

namespace Library;

public interface IAllocationCalculator
{

    decimal GetAllocation(IBotContext context);

}