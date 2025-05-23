using Library.Bot;

namespace Library;

public interface IAllocationScaler
{

    decimal Scale(decimal, IBotContext context);
    decimal CalculateScale(IBotContext context);

}