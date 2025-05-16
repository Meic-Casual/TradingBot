using Library.Trailing;

namespace Library.Bot
{

    public interface IBotContext
    {

        decimal LowestRecordedPrice { get; }
        decimal LastRecordedPrice { get; }
        decimal AveragePurchasePrice { get; }

        decimal OverallAllowance { get; }
        decimal RemainingAllowance { get; }

        IValueTrailing? PriceTrailing { get; }

        IReadOnlyList<decimal> RecordedPrices { get; }

        int PurchasesCount { get; }

    }

}