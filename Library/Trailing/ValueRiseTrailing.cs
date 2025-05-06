namespace Library.Trailing;

public class ValueRiseTrailing : ValueTrailing
{

    protected override decimal DefaultInitialValue => decimal.MinValue;

    public ValueRiseTrailing(decimal triggerValue, float maxReboundRate) : base(triggerValue, maxReboundRate) { }

    public ValueRiseTrailing(decimal triggerValue, float maxReboundRate, decimal initialValue) : base(triggerValue, maxReboundRate, initialValue) { }

    protected override bool IsTriggerThresholdReached(decimal currentValue, decimal newValue) => currentValue < triggerValue && newValue >= triggerValue;

    protected override decimal SelectBestReboundReferencePoint(decimal value1, decimal value2) => decimal.Max(value1, value2);

    protected override float CalculateReboundRate(decimal value) => reboundReferencePoint == 0m ? 0f : (float)decimal.Max((reboundReferencePoint - value) / reboundReferencePoint, 0m);

}
