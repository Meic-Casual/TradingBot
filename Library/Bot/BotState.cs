using Library.Trailing;
using Models.Scaffolded;

namespace Library.Bot
{

    internal class BotState : IBotContext
    {

        internal static BotState CreateInitialStateFrom(IHost<ValueTrailingSettings> settingsProvider)
        {
            ArgumentNullException.ThrowIfNull(settingsProvider);
            if (settingsProvider.Value != null)
            {
                return new BotState()
                {
                    PriceTrailing = settingsProvider.Value.CreateInstanceFromSettings()
                };
            }
            return new();
        }

        public decimal LastRecordedPrice { get; private set; } = 0m;
        public decimal LowestRecordedPrice { get; private set; } = decimal.MaxValue;
        public IReadOnlyList<decimal> RecordedPrices => recordedPrices;

        readonly List<decimal> recordedPrices = [];

        public IReadOnlyList<Purchase> Purchases => purchases;
        public int PurchasesCount => purchases.Count;
        public decimal AveragePurchasePrice => purchases.Count > 0 ? purchases.Average(s => s.Price) : 0m;

        public IValueTrailing? PriceTrailing { get; init; }

        public decimal OverallAllowance => throw new NotImplementedException();
        public decimal RemainingAllowance => throw new NotImplementedException();

        readonly List<Purchase> purchases = [];

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