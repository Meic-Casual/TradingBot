namespace Library;

public readonly struct AllocationScaleEntry(IAllocationScaler source, decimal normalizedScale)
{

    public readonly IAllocationScaler source = source;
    public readonly decimal normalizedScale = normalizedScale;

}