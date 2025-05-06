using Refit;
using System.Globalization;

namespace Library;

// the class supports only limited segment of prices space and clamps it by design - it's mainly for testing and simulation purpose
public class HistoricalChartBasedPriceProvider : ICurrentPriceProvider
{

    int currentReadIndex = 0;

    readonly Lazy<Task<List<decimal>>> cachedChartFetch;

    Task<decimal> ICurrentPriceProvider.FetchCurrentPrice()
    {
        return FetchCurrentPrice();
    }

    public HistoricalChartBasedPriceProvider(int chartDepth, string interval = "15m")
    {
        cachedChartFetch = new(() => FetchChartDataAsync(chartDepth, interval));
    }

    private static async Task<List<decimal>> FetchChartDataAsync(int chartDepth, string interval)
    {
        object[][]? klines;
        var binanceApi = RestService.For<IBinanceAPI>("https://api.binance.com");

        try
        {
            klines = await binanceApi.GetKlinesAsync("BTCUSDT", interval, chartDepth);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to fetch Binance candle data.", ex);
        }

        return TryExtractPrices(klines, out var prices) ? prices : [];
    }

    private static bool TryExtractPrices(object[][]? klines, out List<decimal> prices)
    {
        if (klines != null)
        {
            prices = new List<decimal>(klines.Length);
            foreach (var kline in klines)
            {
                if (decimal.TryParse(kline[4].ToString(), CultureInfo.InvariantCulture, out decimal closeValue))
                {
                    prices.Add(closeValue);
                }
                else
                {
                    Console.WriteLine($"Could not parse close price value: {kline[4]}");
                }
            }
            return true;
        }
        prices = [];
        return false;
    }

    private async Task<decimal> FetchCurrentPrice()
    {
        try
        {
            var prices = await cachedChartFetch.Value;

            if (prices == null || prices.Count == 0)
            {
                Console.WriteLine("No price data available.");
                return ICurrentPriceProvider.InvalidPrice;
            }

            var accessIndex = Math.Min(prices.Count - 1, currentReadIndex++);

            if (currentReadIndex > prices.Count)
            {
                Console.WriteLine("The price space was clamped due to out-of-bounds read request.");
            }

            Console.WriteLine($"FetchCurrentPrice: {prices[accessIndex]}");
            return prices[accessIndex];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to fetch price: {ex.Message}");
            throw;
        }
    }


}