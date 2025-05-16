namespace Library.Trailing;

public interface IValueTrailing
{

    /// <summary>
    /// Indicates whether the trailing condition has been met — i.e., the value has peaked and rebounded by the required rate.
    /// </summary>
    bool IsTrailingConcluded { get; }

    /// <summary>
    /// Indicates whether the trigger point (the peak used to measure the rebound) has been reached.
    /// </summary>
    bool IsTriggerReached { get; }

    decimal LastRecordedValue { get; }
    decimal CurrentReboundValue { get; }

    void UpdateCurrentValue(decimal value, out bool trailingConcluded);
    void UpdateCurrentValue(decimal value);
    void Reset();

}
