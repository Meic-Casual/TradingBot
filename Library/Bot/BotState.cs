using Library.Trailing;
using Models.Scaffolded;

namespace Library.Bot
{

    internal class BotState : IBotContext
    {

        internal static BotState CreateInitialStateFrom(BotSettings botSettings)
        {
            ArgumentNullException.ThrowIfNull(botSettings);
            if (botSettings.TrailingSetting != null)
            {
                return new BotState()
                {
                    PriceTrailing = botSettings.TrailingSetting.CreateInstanceFromSettings()
                };
            }
            return new();
        }

        public decimal LowestRecordedPrice { get; private set; } = decimal.MaxValue;
        public IReadOnlyList<Purchase> Purchases => purchases;
        public IValueTrailing? PriceTrailing { get; init; }

        readonly List<Purchase> purchases = [];

        public void RegisterPriceValue(decimal price)
        {
            LowestRecordedPrice = decimal.Min(price, LowestRecordedPrice);
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