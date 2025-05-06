namespace Library;

public class DcaBotConfig
{
    public decimal InitialAllocation { get; set; } = 100m;
    public decimal TotalBudget { get; set; } = 5000m;
    public decimal DrawdownMultiplier { get; set; } = 1.5m;
    public decimal TargetImpactRatio { get; set; } = 0.05m;
    public decimal BasePaddingPercent { get; set; } = 0.02m;
    public decimal MaxPaddingPercent { get; set; } = 0.06m;
    public string CurveStyle { get; set; } = "quadratic";
    public decimal CurveIntensity { get; set; } = 2.0m;
    public decimal PaddingCurveFactor { get; set; } = 1.0m;
    public decimal ExhaustionBoost { get; set; } = 0.5m;
}
