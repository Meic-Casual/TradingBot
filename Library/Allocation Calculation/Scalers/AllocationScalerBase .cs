using Library.Bot;

namespace Library;

public abstract class AllocationScalerBase : IAllocationScaler
{

    public decimal Scale(decimal baseAllocation, IBotContext context)
    {
        return CalculateScale(context) * baseAllocation;
    }

    public decimal CalculateScale(IBotContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return CalculateScaleSafe(context);
    }

    protected abstract decimal CalculateScaleSafe(IBotContext context);

}
