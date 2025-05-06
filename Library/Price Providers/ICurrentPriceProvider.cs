namespace Library;

public interface ICurrentPriceProvider
{

    public const decimal InvalidPrice = -1m;

    Task<decimal> FetchCurrentPrice();

}