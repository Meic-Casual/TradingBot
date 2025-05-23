namespace Library.Indicators.Tools
{

    public class RSIBasedDipDetector
    {

        readonly byte fallRateDeltaThreshold, stabilizationDeltaThreshold;

        public byte FallDetectionFrameWidth { get; init; } = 3;
        public byte StabilizationDetectionFrameWidth { get; init; } = 3;

        public byte FallDetectionFrameShift { get; init; } = 3;
        public byte StabilizationDetectionFrameShift { get; init; } = 0;

        public bool SingularStabilizationSegmentOnlyAllowed { get; init; } = true;

        int SignedStabilizationDeltaThreshold => -stabilizationDeltaThreshold;

        readonly RSI rsi;

        public RSIBasedDipDetector(RSI rsi, byte fallRateDeltaThreshold, byte stabilizationDeltaThreshold)
        {
            ArgumentNullException.ThrowIfNull(rsi);
            this.rsi = rsi;
            this.fallRateDeltaThreshold = fallRateDeltaThreshold;
            this.stabilizationDeltaThreshold = stabilizationDeltaThreshold;
        }

        public bool IsDipDetected(IReadOnlyList<decimal> prices)
        {
            
            if (FallRate(rsi, prices, FallDetectionFrameShift, FallDetectionFrameWidth) < SignedStabilizationDeltaThreshold)
            {
                return ContainsValidStabilizationSegment(rsi, prices, StabilizationDetectionFrameShift, StabilizationDetectionFrameWidth);
            }
            return false;
        }

        private bool ContainsValidStabilizationSegment(RSI rsi, IReadOnlyList<decimal> prices, byte frameShift, byte frameWidth)
        {
            var deltas = GetValueDeltas(rsi, prices, frameShift, frameWidth);
            return IsStabilizationSegment(deltas.Max()) && StabilizationConstrainsMet(deltas);
        }

        private bool IsStabilizationSegment(float delta) => Math.Abs(delta) <= stabilizationDeltaThreshold;

        private bool StabilizationConstrainsMet(List<float> deltas)// => !SingularStabilizationSegmentOnlyAllowed || deltas.Count(IsStabilizationSegment) == 1;
        {
            var x = !SingularStabilizationSegmentOnlyAllowed;
            var y = deltas.Count(IsStabilizationSegment) == 1;

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"{x} / {y}({deltas.Count(IsStabilizationSegment)} - {string.Join(", ", deltas)})");
            Console.ResetColor();

            return x || y;
        }

        private int FallRate(RSI rsi, IReadOnlyList<decimal> prices, byte frameShift, byte frameWidth)
        {
            var deltas = GetValueDeltas(rsi, prices, frameShift, frameWidth);
            Console.WriteLine($"::::: FallRate({prices.Count}/{deltas.Count}) = {(int)deltas.Min()} from {string.Join(", ", deltas)}");
            return (int)GetValueDeltas(rsi, prices, frameShift, frameWidth).Min();
        }

        private List<float> GetValueDeltas(RSI rsi, IReadOnlyList<decimal> prices, byte frameShift, byte frameWidth)
        {
            var values = new List<float>(frameWidth);
            for (ushort i = 0; i < frameWidth; i++)
            {
                values.Add(rsi.CalculateValue(prices, (ushort)(frameShift + i)));
            }
            values.Reverse();
            return values.Zip(values.Skip(1), (a, b) => b - a).ToList();
        }

    }

}
