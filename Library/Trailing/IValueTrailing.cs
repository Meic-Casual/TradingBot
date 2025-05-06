namespace Library.Trailing;

public interface IValueTrailing
{

    bool TriggerReached { get; }
    decimal CurrentValue { get; }
    decimal CurrentReboundPoint { get; }

    void UpdateCurrentValue(decimal value, out bool trailingConcluded);
    void Reset();

}
