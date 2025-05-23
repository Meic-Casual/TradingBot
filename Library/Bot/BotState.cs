using Library.Trailing;
using Models.Scaffolded;

namespace Library.Bot
{

    internal class BotState : IBotContext
    {

        internal static BotState CreateInitialStateFrom(IBotSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);
            return new BotState()
            {
                PriceTrailing = settings.TrailingSetting?.CreateInstanceFromSettings(),
                AllocationCalculatorState = settings.AllocationCalculatorSettings?.CreateInstanceFromSettings() ?? new()
            };
        }

        public decimal LastRecordedPrice { get; private set; } = 0m;
        public decimal LowestRecordedPrice { get; private set; } = decimal.MaxValue;
        public IReadOnlyList<decimal> RecordedPrices => recordedPrices;

        readonly List<decimal> recordedPrices = new();

        public IReadOnlyList<Purchase> Purchases => purchases;
        public int PurchasesCount => purchases.Count;
        public decimal AveragePurchasePrice => purchases.Count > 0 ? purchases.Average(s => s.Price) : 0m;

        public IValueTrailing? PriceTrailing { get; init; }

        public decimal OverallAllowance => throw new NotImplementedException();
        public decimal RemainingAllowance => throw new NotImplementedException();

        readonly List<Purchase> purchases = [];

        public AllocationCalculatorState AllocationCalculatorState { get; private init; }

        public void RegisterPriceValue(decimal price)
        {
            LowestRecordedPrice = decimal.Min(price, LowestRecordedPrice);
            LastRecordedPrice = price;
            recordedPrices.Add(price);
            PriceTrailing?.UpdateCurrentValue(price);
        }

        public void RegisterPurchase(Purchase purchase) => purchases.Add(purchase);

        internal void Reset()
        {
            purchases.Clear();
            LowestRecordedPrice = decimal.MaxValue;
            PriceTrailing?.Reset();
        }

    }

}