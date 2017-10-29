using BitCoin_Advisor.Helpers;
using BitCoin_Advisor.Models;
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
    public class ArbitrageLoader
    {
        ObservableRangeCollection<Arbitrage> arbitrages = new ObservableRangeCollection<Arbitrage>();
        ConcurrentBag<Source> sources = new ConcurrentBag<Source>();

        public ArbitrageLoader()
        {

        }

        public async Task<ObservableRangeCollection<Arbitrage>> LoadSources()
        {
            arbitrages.Clear();

            var sourceMB = new Source() { Name = "MB", Image = @"BitCoin_Advisor.Images.mb.png", TickerUrl = "https://www.mercadobitcoin.net/api/BTC/ticker/", Fee = 0.97m, Currency = "BRL" };
            var sourceBistamp = new Source() { Name = "BitStamp", Image = @"BitCoin_Advisor.Images.bitstamp.png", TickerUrl = "https://www.bitstamp.net/api/ticker/", Fee = 0.99m, Currency = "USD" };
            var sourceFoxBit = new Source() { Name = "FoxBit", Image = @"BitCoin_Advisor.Images.foxbit.png", TickerUrl = "https://api.blinktrade.com/api/v1/BRL/ticker", Fee = 0.98m, Currency = "BRL" };
            var sourceCoinfloor = new Source() { Name = "CoinFloor", Image = @"BitCoin_Advisor.Images.coinfloor.png", TickerUrl = "https://webapi.coinfloor.co.uk:8090/bist/XBT/GBP/ticker/", Fee = 0.99m, Currency = "GBP" };

            sources.Add(sourceMB);
            sources.Add(sourceBistamp);
            sources.Add(sourceFoxBit);
            sources.Add(sourceCoinfloor);

            var tasks = sources.Select(async item =>
            {
                await GetTicker(item);
            });
            await Task.WhenAll(tasks);

            //decimal usdBrl = await GetUsdBrl();

            BLExchangeRate bLExchangeRate = new BLExchangeRate();
            


            arbitrages.Add(new Arbitrage(sourceBistamp, sourceFoxBit, 1000, await bLExchangeRate.GetExchangeRate(sourceBistamp.Currency, sourceFoxBit.Currency)));
            arbitrages.Add(new Arbitrage(sourceBistamp, sourceMB, 1000, await bLExchangeRate.GetExchangeRate(sourceBistamp.Currency, sourceMB.Currency)));
            arbitrages.Add(new Arbitrage(sourceCoinfloor, sourceMB, 1000, await bLExchangeRate.GetExchangeRate(sourceCoinfloor.Currency, sourceMB.Currency)));
            arbitrages.Add(new Arbitrage(sourceCoinfloor, sourceBistamp, 1000, await bLExchangeRate.GetExchangeRate(sourceCoinfloor.Currency, sourceBistamp.Currency)));
            arbitrages.Add(new Arbitrage(sourceCoinfloor, sourceFoxBit, 1000, await bLExchangeRate.GetExchangeRate(sourceCoinfloor.Currency, sourceFoxBit.Currency)));
            arbitrages.Add(new Arbitrage(sourceMB, sourceCoinfloor, 1000, await bLExchangeRate.GetExchangeRate(sourceMB.Currency, sourceCoinfloor.Currency)));
            arbitrages.Add(new Arbitrage(sourceFoxBit, sourceCoinfloor, 1000, await bLExchangeRate.GetExchangeRate(sourceFoxBit.Currency, sourceCoinfloor.Currency)));

            return arbitrages;
        }


        public async Task GetTicker(Source source)
        {
            try
            {
                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var address = String.Format(source.TickerUrl);
                var response = await client.GetAsync(address);
                var resultJson = response.Content.ReadAsStringAsync().Result;

                if (!resultJson.Contains("ticker"))
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
