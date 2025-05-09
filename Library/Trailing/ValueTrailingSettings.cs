namespace Library.Trailing;

public class ValueTrailingSettings
{

    public enum ChangeType { RISING, FALLING }

    public readonly ChangeType valuePrimaryChangeType;
    public readonly decimal triggerValue;
    public readonly float maxReboundRate;
    public readonly decimal? initialValue;

    public ValueTrailingSettings(ChangeType valuePrimaryChangeType, decimal triggerValue, float maxReboundRate)
    {
        this.valuePrimaryChangeType = valuePrimaryChangeType;
        this.triggerValue = triggerValue;
        this.maxReboundRate = maxReboundRate;
    }

    public ValueTrailingSettings(ChangeType valuePrimaryChangeType, decimal triggerValue, float maxReboundRate, decimal initialValue) : this(valuePrimaryChangeType, triggerValue, maxReboundRate)
    {
        this.initialValue = initialValue;
    }

    public IValueTrailing CreateInstanceFromSettings()
    {
        switch (valuePrimaryChangeType)
        {
            case ChangeType.RISING:
                return initialValue.HasValue ? new ValueRiseTrailing(triggerValue, maxReboundRate, initialValue.Value) : new ValueRiseTrailing(triggerValue, maxReboundRate);
            case ChangeType.FALLING:
                return initialValue.HasValue ? new ValueFallTrailing(triggerValue, maxReboundRate, initialValue.Value) : new ValueFallTrailing(triggerValue, maxReboundRate);
        }
        throw new NotImplementedException($"Instance creation for {valuePrimaryChangeType} not implemented.");
    }

}
