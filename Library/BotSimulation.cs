using Library.Trailing;
using Models.Scaffolded;

namespace Library
{
    public class BotSimulation
    {

        private const decimal NoPurchasesYet = decimal.MaxValue;

        decimal initialPurchaseQuota;
        int candlesDepth;
        readonly ICurrentPriceProvider priceProvider;

        IValueTrailing? priceTrailing;

        //tmp
        decimal buyOverallCost = 0m, sellOverallCost = 0m;

        public BotSimulation(decimal initialPurchaseQuota, ICurrentPriceProvider priceProvider, int candlesDepth, IValueTrailing? valueTrailing = null)
        {
            ArgumentNullException.ThrowIfNull(priceProvider);
            this.initialPurchaseQuota = initialPurchaseQuota;
            this.candlesDepth = candlesDepth;
            this.priceProvider = priceProvider;
            priceTrailing = valueTrailing;
        }

        public async Task Run()
        {
            List<decimal> purchases = [];
            for (int i = 0; i < candlesDepth; i++)
            {
                decimal currentPrice;

                try
                {
                    currentPrice = await priceProvider.FetchCurrentPrice();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to fetch current price: {ex.Message}");
                    return;
                }

                var avgPrice = purchases.Count > 0 ? purchases.Average() : NoPurchasesYet;
                
                if (avgPrice > currentPrice)
                {
                    purchases.Add(currentPrice);
                    Console.WriteLine($"Price ({currentPrice}) lower than AVG ({avgPrice}) - purchase.");
                }
                else
                {
                    if(TrySelling(currentPrice, avgPrice))
                    {
                        purchases.Clear();
                        priceTrailing?.Reset();
                    }
                }
            }
        }

        private bool TrySelling(decimal currentPrice, decimal avgPrice)
        {
            UpdateTrailingPrice(currentPrice, out var priceSuitableForSell);
            var profitRate = (currentPrice - avgPrice) / avgPrice * 100m;
            if (profitRate > 0)
            {
                if (priceSuitableForSell)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Profit: {profitRate:F2} (from {currentPrice} - {avgPrice}) - selling.");
                    Console.ResetColor();
                    return true;
                }
                else
                {
                    Console.WriteLine($"Profit: {profitRate:F2} but trailing prevents sell.");
                }
            }
            else
            {
                Console.WriteLine($"Price ({currentPrice}) higher than AVG ({avgPrice}) - skip ({profitRate:F2}).");
            }
            return false;
        }

        void UpdateTrailingPrice(decimal price, out bool priceSuitableForSell)
        {
            if (priceTrailing != null)
            {
                priceTrailing.UpdateCurrentValue(price, out priceSuitableForSell);
            }
            else
            {
                //trailing is not used, so no additional requirements for sell exist
                priceSuitableForSell = true;
            }
        }

    }
}
