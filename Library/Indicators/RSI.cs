namespace Library.Indicators
{

    public class RSI(int periods = 14)
    {

        public const float UnsufficientDataResponse = -1f;

        readonly int periods = periods;

        public float CalculateValue(IReadOnlyList<decimal> prices)
        {
            ArgumentNullException.ThrowIfNull(prices);
            if (prices.Count >= periods)
            {
                decimal gainsSum = 0m;
                decimal lossesSum = 0m;
                
                for (int i = 1; i < periods; i++)
                {
                    var change = prices[^(i - 1)] - prices[^i];
                    gainsSum += decimal.Max(change, 0m);
                    lossesSum += Math.Abs(decimal.Min(change, 0m));
                }

                var avgLoss = lossesSum / periods;
                if (avgLoss == 0) return 100f;

                var avgGain = gainsSum / periods;
                var rs = (float)(avgGain / avgLoss);

                return 100f - (100f / (1f + rs));
            }
            return UnsufficientDataResponse;
        }
    }

}