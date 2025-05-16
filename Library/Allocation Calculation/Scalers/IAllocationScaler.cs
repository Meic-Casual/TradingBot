using Library.Bot;

namespace Library;

public interface IAllocationScaler
{

    decimal Scale(decimal baseAllocation, IBotContext context);
    decimal CalculateScale(IBotContext context);

}