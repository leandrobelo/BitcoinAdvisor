using BitCoin_Advisor.Helpers;
using BitCoin_Advisor.Models;
using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BitCoin_Advisor.Business
{
    public static class ArbitrageLoader
    {
        private static object loadLock = new object();
        private static ObservableRangeCollection<Arbitrage> arbitrages = new ObservableRangeCollection<Arbitrage>();
        private static ConcurrentBag<ExchangeTicker> sources = new ConcurrentBag<ExchangeTicker>();

        private static ObservableRangeCollection<Exchange> exchanges = new ObservableRangeCollection<Exchange>();
        public static ObservableRangeCollection<Exchange> Exchanges
        {
            get { return exchanges; }
        }

        public static ObservableRangeCollection<Arbitrage> Arbitrages
        {
            get { return arbitrages; }
        }

        public static async Task LoadSources()
        {
            lock (loadLock)
            {
                if (sources.Count == 0)
                {
                    var exchangeMB = new Exchange() { Name = "Mercado Bitcoin", Image = @"BitCoin_Advisor.Images.mb.png", Fee = 0.99m, Currency = "BRL", IsEnabled = true };
                    var exchangeBitCoinTrade = new Exchange() { Name = "BitCoin Trade", Image = @"BitCoin_Advisor.Images.bitcointrade.png", Fee = 0.99m, Currency = "BRL", IsEnabled = true };
                    var exchangeBistamp = new Exchange() { Name = "BitStamp", Image = @"BitCoin_Advisor.Images.bitstamp.png", Fee = 0.99m, Currency = "USD", IsEnabled = true };
                    var exchangeFoxBit = new Exchange() { Name = "FoxBit", Image = @"BitCoin_Advisor.Images.foxbit.png", Fee = 0.995m, Currency = "BRL", IsEnabled = true };
                    var exchangeCoinfloor = new Exchange() { Name = "CoinFloor", Image = @"BitCoin_Advisor.Images.coinfloor.png", Fee = 0.99m, Currency = "GBP", IsEnabled = true };

                    exchanges = new ObservableRangeCollection<Exchange>() { exchangeMB, exchangeBitCoinTrade, exchangeBistamp, exchangeFoxBit, exchangeCoinfloor };

                    var btc = new Symbol() { Name = "BTC" };

                    var sourceMB = new ExchangeTicker() { Exchange = exchangeMB, TickerUrl = "https://www.mercadobitcoin.net/api/BTC/ticker/", Symbol = btc };
                    var sourceBitCoinTrade = new ExchangeTicker() { Exchange = exchangeBitCoinTrade, TickerUrl = "https://api.bitcointrade.com.br/v1/public/BTC/ticker", Symbol = btc };
                    var sourceBistamp = new ExchangeTicker() { Exchange = exchangeBistamp, TickerUrl = "https://www.bitstamp.net/api/ticker/", Symbol = btc };
                    var sourceFoxBit = new ExchangeTicker() { Exchange = exchangeFoxBit, TickerUrl = "https://api.blinktrade.com/api/v1/BRL/ticker", Symbol = btc };
                    var sourceCoinfloor = new ExchangeTicker() { Exchange = exchangeCoinfloor, TickerUrl = "https://webapi.coinfloor.co.uk:8090/bist/XBT/GBP/ticker/", Symbol = btc };

                    //sources.Add(sourceCoinfloor);
                    sources.Add(sourceBistamp);
                    sources.Add(sourceMB);
                    sources.Add(sourceBitCoinTrade);
                    sources.Add(sourceFoxBit);
                }

                if (arbitrages.Count == 0)
                {
                    foreach (var s in sources)
                    {
                        foreach (var s2 in sources)
                        {
                            if (s != s2)
                                arbitrages.Add(new Arbitrage(s, s2));
                        }
                    }
                }
            }

            var tasks = sources.Select(async item =>
            {
                await GetTicker(item);
            });
            await Task.WhenAll(tasks);

            BLExchangeRate bLExchangeRate = new BLExchangeRate();

            foreach (var arbitrage in arbitrages)
            {
                arbitrage.Conversion = await bLExchangeRate.GetExchangeRate(arbitrage.From.Exchange.Currency, arbitrage.To.Exchange.Currency);
                arbitrage.CalculateNet();
            }
        }

        public static async Task GetTicker(ExchangeTicker source)
        {
            try
            {
                var client = new HttpClient(new NativeMessageHandler());
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromMinutes(300);

                var address = String.Format(source.TickerUrl);
                var response = await client.GetAsync(address);


                var resultJson = response.Content.ReadAsStringAsync().Result;

                if (resultJson.Contains("\"data\""))
                {
                    resultJson = resultJson.Replace("\"data\"", "ticker");
                }
                else if (!resultJson.Contains("ticker"))
                {
                    resultJson = @"{""ticker"": " + resultJson + "}";
                }

                var jsonObject = JsonConvert.DeserializeObject<Tickers>(resultJson);
                source.Price = jsonObject.ticker;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
