namespace Library.Trailing;

public class ValueFallTrailing : ValueTrailing
{

    protected override decimal DefaultInitialValue => decimal.MaxValue;

    public ValueFallTrailing(decimal triggerValue, float maxReboundRate) : base(triggerValue, maxReboundRate) { }

    public ValueFallTrailing(decimal triggerValue, float maxReboundRate, decimal initialValue) : base(triggerValue, maxReboundRate, initialValue) { }

    protected override bool IsTriggerThresholdReached(decimal currentValue, decimal newValue) => currentValue > triggerValue && newValue <= triggerValue;

    protected override decimal SelectBestReboundReferencePoint(decimal value1, decimal value2) => decimal.Min(value1, value2);

    protected override float CalculateReboundRate(decimal value) => reboundReferencePoint == 0m ? 0f : (float)decimal.Max((value - reboundReferencePoint) / reboundReferencePoint, 0m);

}
