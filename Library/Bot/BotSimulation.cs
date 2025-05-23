using Models.Scaffolded;

namespace Library.Bot
{

    public class BotSimulation
    {

        private const decimal NoPurchasesYet = decimal.MaxValue;

        decimal BaseStepFunds => botSettings.BaseStepFunds;

        readonly int candlesDepth;
        readonly ICurrentPriceProvider priceProvider;
        
        readonly BotSettings botSettings;
        readonly BotState botState;
        readonly List<IActionSkipCondition> purchaseSkipConditions;
        readonly List<IActionSkipCondition> sellSkipConditions;
        readonly AllocationCalculator? allocationCalculator;

        public BotSimulation(BotSettings botSettings, ICurrentPriceProvider priceProvider, int candlesDepth)
        {
            ArgumentNullException.ThrowIfNull(botSettings);
            ArgumentNullException.ThrowIfNull(priceProvider);

            this.candlesDepth = candlesDepth;
            this.priceProvider = priceProvider;
            this.botSettings = botSettings;

            botState = BotState.CreateInitialStateFrom(botSettings);
            sellSkipConditions = SellConditionFactory.FromSettings(botSettings).ToList();
            purchaseSkipConditions = PurchaseConditionFactory.FromSettings(botSettings).ToList();
        }

        Indicators.Tools.RSIBasedDipDetector detector = new Indicators.Tools.RSIBasedDipDetector(new Indicators.RSI(), 15, 8);

        public async Task Run()
        {
            Console.ForegroundColor = ConsoleColor.White;
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

                botState.RegisterPriceValue(currentPrice);
                var avgPrice = botState.PurchasesCount > 0 ? botState.AveragePurchasePrice : NoPurchasesYet;

                Console.WriteLine(" >>> " + (detector.IsDipDetected(botState.RecordedPrices) ? "dip detected at " + currentPrice : "no dip detected"));

                //test
                if(new DipScaler(new Indicators.Tools.RSIBasedDipDetector(new(), 10, 5)).CalculateScale(botState) > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine($"DIP (X) DETECTED for {currentPrice}!");
                    Console.ResetColor();
                }
                if (new DipScaler(new Indicators.Tools.RSIBasedDipDetector(new(), 10, 5) { SingularStabilizationSegmentOnlyAllowed = false }).CalculateScale(botState) > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"DIP DETECTED for {currentPrice}!");
                    Console.ResetColor();
                }
                //new OversoldScaler().CalculateScale(botState);

                if (avgPrice > currentPrice)
                {
                    if (IsSuitableForPurchase(botState))
                    {
                        botState.RegisterPurchase(new Purchase(currentPrice, BaseStepFunds));
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Price ({currentPrice}) lower than AVG ({avgPrice}) - purchase - {botState.Purchases.Count} purchases in register.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    if (IsSuitableForSell(botState))
                    {
                        //test
                        var overallCost = botState.Purchases.Sum(s => s.Cost);
                        var overallAssetsQuantity = botState.Purchases.Sum(s => s.Quantity);
                        var grossProfit = overallAssetsQuantity * currentPrice - overallCost;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"$$$ Gross profit is {grossProfit:F2} (from {overallCost:F2}).");
                        Console.ResetColor();
                        //
                        botState.Reset();
                    }
                }
            }
        }

        private bool IsSuitableForSell(IBotContext context) => sellSkipConditions.All(s => !s.SkipConditionMet(context));
        private bool IsSuitableForPurchase(IBotContext context) => purchaseSkipConditions.All(s => !s.SkipConditionMet(context));

    }

}