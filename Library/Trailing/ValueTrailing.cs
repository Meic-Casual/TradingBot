namespace Library.Trailing;

public abstract class ValueTrailing : IValueTrailing
{

    protected readonly decimal triggerValue;
    private readonly float maxReboundRate;

    protected decimal reboundReferencePoint;
    decimal currentValue;
    bool triggerThresholdReached = false;

    protected abstract decimal DefaultInitialValue { get; }

    public bool TriggerReached => triggerThresholdReached;
    public decimal CurrentValue => currentValue;
    public decimal CurrentReboundPoint => reboundReferencePoint;

    /// <param name="maxReboundRate">Normalized value.</param>
    public ValueTrailing(decimal triggerValue, float maxReboundRate)
    {
        this.triggerValue = triggerValue;
        this.maxReboundRate = maxReboundRate;
        ResetToInitialValues();
    }

    void ResetToInitialValues()
    {
        reboundReferencePoint = triggerValue;
        currentValue = DefaultInitialValue;
    }

    /// <param name="maxReboundRate">Normalized value.</param>
    public ValueTrailing(decimal triggerValue, float maxReboundRate, decimal initialValue) : this(triggerValue, maxReboundRate)
    {
        currentValue = initialValue;
    }

    public void UpdateCurrentValue(decimal value, out bool trailingConcluded)
    {
        trailingConcluded = false;
        triggerThresholdReached |= IsTriggerThresholdReached(currentValue, value);
        Console.WriteLine($"triggerThresholdReached: {triggerThresholdReached} (from {currentValue} vs {value})");
        currentValue = value;
        if (triggerThresholdReached)
        {
            Console.WriteLine($"reboundReferencePoint update from from {reboundReferencePoint} and {value}");
            reboundReferencePoint = SelectBestReboundReferencePoint(reboundReferencePoint, value);
            Console.WriteLine($"reboundReferencePoint set to {reboundReferencePoint}");
            Console.WriteLine($"rebound rate comparison: {CalculateReboundRate(value)} vs {maxReboundRate}");
            trailingConcluded = CalculateReboundRate(value) >= maxReboundRate;
        }
    }

    protected abstract bool IsTriggerThresholdReached(decimal currentValue, decimal newValue);
    protected abstract decimal SelectBestReboundReferencePoint(decimal value1, decimal value2);
    protected abstract float CalculateReboundRate(decimal value);

    public void Reset() => ResetToInitialValues();

}
