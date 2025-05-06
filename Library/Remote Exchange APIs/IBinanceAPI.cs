using Refit;

namespace Library;

public interface IBinanceAPI
{

    // Fetch Kline (candlestick) data for a given symbol and interval
    [Get("/api/v3/klines")]
    Task<object[][]> GetKlinesAsync(
        [AliasAs("symbol")] string symbol,
        [AliasAs("interval")] string interval,
        [AliasAs("limit")] int limit = 100
    );

    // Fetch current market price
    [Get("/api/v3/ticker/price")]
    Task<PriceResponse> GetPriceAsync(
        [AliasAs("symbol")] string symbol
    );

}
