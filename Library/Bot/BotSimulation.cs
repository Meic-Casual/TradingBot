using Library.Trailing;
using Models.Scaffolded;

namespace Library.Bot
{
    public class BotSimulation
    {

        private const decimal NoPurchasesYet = decimal.MaxValue;

        decimal BaseStepFunds => botSettings.BaseStepFunds;
        decimal MinProfitRatePercent => botSettings.MinProfitRatePercent;
        IValueTrailing? PriceTrailing => currentBotState.PriceTrailing;

        readonly int candlesDepth;
        readonly ICurrentPriceProvider priceProvider;
        
        readonly BotSettings botSettings;
        readonly BotState currentBotState;

        public BotSimulation(BotSettings botSettings, ICurrentPriceProvider priceProvider, int candlesDepth)
        {
            ArgumentNullException.ThrowIfNull(botSettings);
            ArgumentNullException.ThrowIfNull(priceProvider);

            this.candlesDepth = candlesDepth;
            this.priceProvider = priceProvider;
            this.botSettings = botSettings;

            currentBotState = BotState.CreateInitialStateFrom(botSettings);
        }

        public async Task Run()
        {
            var purchases = currentBotState.Purchases;
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

                var avgPrice = purchases.Count > 0 ? purchases.Average(s => s.Price) : NoPurchasesYet;
                
                if (avgPrice > currentPrice)
                {
                    currentBotState.RegisterPurchase(new Purchase(currentPrice, BaseStepFunds));
                    Console.WriteLine($"Price ({currentPrice}) lower than AVG ({avgPrice}) - purchase.");
                }
                else
                {
                    if(TrySelling(currentPrice, avgPrice))
                    {
                        //test
                        var overallCost = purchases.Sum(s => s.Cost);
                        var overallAssetsQuantity = purchases.Sum(s => s.Quantity);
                        var grossProfit = overallAssetsQuantity * currentPrice - overallCost;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Gross profit is {grossProfit:F2} (from {overallCost:F2}).");
                        Console.ResetColor();
                        //
                        currentBotState.Reset();
                    }
                }
            }
        }

        private bool TrySelling(decimal currentPrice, decimal avgPrice)
        {
            UpdateTrailingPrice(currentPrice, out var priceSuitableForSell);
            var profitRate = (currentPrice - avgPrice) / avgPrice * 100m;
            if (profitRate >= MinProfitRatePercent)
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
            if (PriceTrailing != null)
            {
                PriceTrailing.UpdateCurrentValue(price, out priceSuitableForSell);
            }
            else
            {
                //trailing is not used, so no additional requirements for sell exist
                priceSuitableForSell = true;
            }
        }

    }
}
