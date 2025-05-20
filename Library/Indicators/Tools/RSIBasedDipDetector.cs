namespace Library.Indicators.Tools
{

    internal class RSIBasedDipDetector
    {

        readonly ushort fallRateDeltaThreshold, stabilizationDeltaThreshold;

        public ushort FallDetectionFrameWidth { get; init; } = 3;
        public ushort StabilizationDetectionFrameWidth { get; init; } = 3;

        public ushort FallDetectionFrameShift { get; init; } = 3;
        public ushort StabilizationDetectionFrameShift { get; init; } = 0;

        readonly RSI rsi;

        public RSIBasedDipDetector(RSI rsi, ushort fallRateDeltaThreshold, ushort stabilizationDeltaThreshold)
        {
            ArgumentNullException.ThrowIfNull(rsi);
            this.rsi = rsi;
            this.fallRateDeltaThreshold = fallRateDeltaThreshold;
            this.stabilizationDeltaThreshold = stabilizationDeltaThreshold;
        }

        public bool DipDetected(IReadOnlyList<decimal> prices)
        {
            if (FallRate(rsi, prices, FallDetectionFrameShift, FallDetectionFrameWidth) >= fallRateDeltaThreshold)
            {
                return RiseRate(rsi, prices, StabilizationDetectionFrameShift, StabilizationDetectionFrameWidth) <= stabilizationDeltaThreshold;
            }
            return false;
        }

        private int RiseRate(RSI rsi, IReadOnlyList<decimal> prices, ushort frameShift, ushort frameWidth)
        {
            return (int)GetValueDeltas(rsi, prices, frameShift, frameWidth).Max();
        }

        private int FallRate(RSI rsi, IReadOnlyList<decimal> prices, ushort frameShift, ushort frameWidth)
        {
            return (int)Math.Abs(GetValueDeltas(rsi, prices, frameShift, frameWidth).Min());
        }

        private List<float> GetValueDeltas(RSI rsi, IReadOnlyList<decimal> prices, ushort frameShift, ushort frameWidth)
        {
            var values = new List<float>(frameWidth);
            for (ushort i = 0; i < frameWidth; i++)
            {
                values.Add(rsi.CalculateValue(prices, i));
            }
            values.Reverse();
            return values.Zip(values.Skip(1), (a, b) => b - a).ToList();
        }

    }

}
