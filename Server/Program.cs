using Library;
using Library.Trailing;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Scaffolded;

internal class Program
{

    const int userId = 1;

    static async Task Main(string[] args)
    {
        await new App(new DcaBotContextFactory().CreateDbContext(), userId).RunAsync();
    }

    class App(DcaBotContext dcaContext, int userId)
    {

        public async Task RunAsync()
        {
            while (true)
            {
                Console.WriteLine("Pick action");
                Console.WriteLine("1. List bots");
                Console.WriteLine("2. Create bot");
                Console.WriteLine("3. Delete bot");
                Console.WriteLine("4. Run simulation");
                Console.WriteLine("5. Read chart data");
                Console.WriteLine("6. Read current price");
                Console.WriteLine("Q. Quit");
                var input = Console.ReadLine();
                if (input?.TrimEnd('.').ToLower() == "q") break;
                var selection = Convert.ToInt16(input);
                switch (selection)
                {
                    case 1:
                        await ListBots();
                        break;
                    case 2:
                        await CreateBot();
                        break;
                    case 3:
                        await DeleteBot();
                        break;
                    case 4:
                        await SimulateBot();
                        break;
                    case 5:
                        await ReadChartData();
                        break;
                    case 6:
                        await ReadCurrentPrice();
                        break;
                }
            }
        }

        readonly CurrentPriceFromBinanceProvider currentPriceFromBinanceProvider = new();

        private async Task ReadCurrentPrice()
        {
            var price = await (currentPriceFromBinanceProvider as ICurrentPriceProvider).FetchCurrentPrice();
            Console.WriteLine($"Current price: {price}");
        }

        private async Task ReadChartData()
        {
            var priceProvider = new HistoricalChartBasedPriceProvider(10) as ICurrentPriceProvider;
            while (true)
            {
                Console.WriteLine("Print next? (y/n)");
                var c = Console.ReadKey();
                if (c.KeyChar == 'y')
                {
                    var price = await priceProvider.FetchCurrentPrice();
                    Console.WriteLine($"Fetched price: {price}");
                }
                else if (c.KeyChar == 'n')
                    break;
                else
                    Console.WriteLine("Invalid input.");
            }
        }

        private async Task SimulateBot()
        {
            Console.WriteLine("Set simulation depth:");
            int simDepth = 50;
            while (!int.TryParse(Console.ReadLine(), out simDepth))
            {
                Console.WriteLine("Invalid input. Try again.");
            }
            Console.WriteLine($"Simulation depth set to {simDepth}");
            Console.WriteLine("set interval (1m,3m,5m,15m,30m,1h,2h,4h,6h,8h,12h,1d,3d,1w,1M):");
            var interval = Console.ReadLine() ?? "15m";
            var botSim = new BotSimulation(10, new HistoricalChartBasedPriceProvider(simDepth, interval), simDepth, new ValueRiseTrailing(95000m, 0.005f));
            await botSim.Run();
        }

        private async Task DeleteBot()
        {
            Console.WriteLine($"Set id of bot to delete:");
            int botId = default;
            while (!int.TryParse(Console.ReadLine(), out botId))
            {
                Console.WriteLine("Invalid input. Try again.");
            }
            await dcaContext.Bots.RemoveMatchingAsync(s => s.OwnerId == userId && s.Id == botId);
            await dcaContext.SaveChangesAsync();
        }

        async Task ListBots()
        {
            var bots = await dcaContext.Bots.Where(s => s.OwnerId == userId).ToListAsync();
            Console.WriteLine($"Existing bots count: {bots.Count}");
            foreach (var bot in bots)
            {
                Console.WriteLine($"bot {bot.Id} - State - 0$/{bot.OverallAllowance}$");
            }
        }

        async Task CreateBot()
        {
            Console.WriteLine("Set allowance quantity ($):");
            decimal allowance = default;
            while (!Decimal.TryParse(Console.ReadLine(), out allowance))
            {
                Console.WriteLine("Invalid input. Try again.");
            }
            Console.WriteLine($"Allowance set to {allowance}$");
            await dcaContext.Bots.AddAsync(new Bot() { OwnerId = userId, OverallAllowance = allowance });
            await dcaContext.SaveChangesAsync();
        }

    }

}