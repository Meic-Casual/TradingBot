using Refit;
using System.Globalization;

namespace Library;

public class CurrentPriceFromBinanceProvider : ICurrentPriceProvider
{

    readonly SemaphoreSlim fetchLock = new(1, 1);
    Task<decimal>? currentFetch;
    
    Task<decimal> ICurrentPriceProvider.FetchCurrentPrice()
    {
        return FetchCurrentPrice();
    }

    private static async Task<decimal> FetchRemotePriceAsync()
    {
        var binanceApi = RestService.For<IBinanceAPI>("https://api.binance.com");
        var priceData = await binanceApi.GetPriceAsync("BTCUSDT");
        return decimal.TryParse(priceData?.Price, CultureInfo.InvariantCulture, out var price)
        ? price
        : throw new InvalidOperationException("Price is null or not a valid decimal.");

    }

    private async Task<decimal> FetchCurrentPrice()
    {
        await fetchLock.WaitAsync();
        try
        {
            if (currentFetch == null)
            {
                currentFetch = FetchRemotePriceAsync();
            }
            var result = await currentFetch;
            currentFetch = null;
            return result;
        }
        finally
        {
            fetchLock.Release();
        }
    }

}