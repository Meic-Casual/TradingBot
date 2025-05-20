namespace Library.Indicators
{

    public class RSI(int periods = 14)
    {

        public const float InsufficientDataResponse = -1f;

        readonly int periods = periods;

        /// <param name="readIndex">
        /// Offset from the end of the price list to specify which point in time to calculate the RSI for.
        /// A value of 0 calculates RSI using the most recent prices, 1 uses data one step back, and so on.
        /// </param>
        public float CalculateValue(IReadOnlyList<decimal> prices, ushort readIndex)
        {
            ArgumentNullException.ThrowIfNull(prices);
            if (prices.Count >= periods + 1 + readIndex)
            {
                decimal gainsSum = 0m;
                decimal lossesSum = 0m;

                for (int i = 0; i < periods; i++)
                {
                    var change = prices[^(i + 1 + readIndex)] - prices[^(i + 2 + readIndex)];
                    gainsSum += decimal.Max(change, 0m);
                    lossesSum += Math.Abs(decimal.Min(change, 0m));
                }

                var avgLoss = lossesSum / periods;
                if (avgLoss == 0) return 100f;

                var avgGain = gainsSum / periods;
                var rs = (float)(avgGain / avgLoss);

                return 100f - (100f / (1f + rs));
            }
            return InsufficientDataResponse;
        }

        public float CalculateValue(IReadOnlyList<decimal> prices)
        {
            return CalculateValue(prices, 0);
        }
    }

}