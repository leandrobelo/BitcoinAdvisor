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

            var sourceMB = new Source() { Name = "MB", Image = @"BitCoin_Advisor.Images.mb.png", TickerUrl = "https://www.mercadobitcoin.net/api/BTC/ticker/", Fee = 0.99m, Currency = "BRL" };
            var sourceBitCoinTrade = new Source() { Name = "BitCoinTrade", Image = @"BitCoin_Advisor.Images.bitcointrade.jpg", TickerUrl = "https://api.bitcointrade.com.br/v1/public/BTC/ticker", Fee = 0.99m, Currency = "BRL" };            
            var sourceBistamp = new Source() { Name = "BitStamp", Image = @"BitCoin_Advisor.Images.bitstamp.png", TickerUrl = "https://www.bitstamp.net/api/ticker/", Fee = 0.99m, Currency = "USD" };
            var sourceFoxBit = new Source() { Name = "FoxBit", Image = @"BitCoin_Advisor.Images.foxbit.png", TickerUrl = "https://api.blinktrade.com/api/v1/BRL/ticker", Fee = 0.995m, Currency = "BRL" };
            var sourceCoinfloor = new Source() { Name = "CoinFloor", Image = @"BitCoin_Advisor.Images.coinfloor.png", TickerUrl = "https://webapi.coinfloor.co.uk:8090/bist/XBT/GBP/ticker/", Fee = 0.99m, Currency = "GBP" };

//            sources.Add(sourceCoinfloor);
            sources.Add(sourceBistamp);
            sources.Add(sourceMB);
            sources.Add(sourceBitCoinTrade);
            sources.Add(sourceFoxBit);
            

            var tasks = sources.Select(async item =>
            {
                await GetTicker(item);
            });
            await Task.WhenAll(tasks);
            
            BLExchangeRate bLExchangeRate = new BLExchangeRate();
            

            foreach( var s in sources)
            {
                foreach( var s2 in sources)
                {
                    if (s != s2)
                    {
                        arbitrages.Add(new Arbitrage(s, s2));
                    }
                }
            }

            foreach(var arbitrage in arbitrages)
            {
                arbitrage.Conversion = await bLExchangeRate.GetExchangeRate(arbitrage.From.Currency, arbitrage.To.Currency);
            }

            return arbitrages;
        }


        public async Task GetTicker(Source source)
        {
            try
            {
                var client = new HttpClient();
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
                //return new Task() { return new Tickers()};
            }
        }
    }
}
