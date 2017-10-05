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

            var sourceMB = new Source() { Name = "MB", Image = @"Images\mb.png", TickerUrl = "https://www.mercadobitcoin.net/api/BTC/ticker/", Fee = 0.97m };
            var sourceBistamp = new Source() { Name = "BitStamp", Image = @"Images\bistamp.png", TickerUrl = "https://www.bitstamp.net/api/ticker/", Fee = 0.99m };
            var sourceFoxBit = new Source() { Name = "FoxBit", Image = @"Images\foxbit.png", TickerUrl = "https://api.blinktrade.com/api/v1/BRL/ticker", Fee = 0.98m };

            sources.Add(sourceMB);
            sources.Add(sourceBistamp);
            sources.Add(sourceFoxBit);

            foreach (var source in sources)
            {
                await GetTicker(source);
            }

            decimal usdBrl = await GetUsdBrl();

            arbitrages.Add(new Arbitrage() { From = sourceBistamp, To = sourceFoxBit, Conversion = usdBrl, Capital = 1000 });
            arbitrages.Add(new Arbitrage() { From = sourceBistamp, To = sourceMB, Conversion = usdBrl, Capital = 1000 });

            return arbitrages;
        }

        private async Task<decimal> GetUsdBrl()
        {
            string exchangeUrl = "https://www.ebanx.com/business/en/dashboard/exchange";

            var byteUrl = await ParseExchange(exchangeUrl);
            string regEx = "<h3>USD 1 = BRL ([0-9]*\\.*[0-9]*)</h3>";
            Regex regex = new Regex(regEx);
            string result = System.Text.Encoding.UTF8.GetString(byteUrl, 0, byteUrl.Count() - 1);
            var matched = regex.Match(result);
            decimal UsdBrl = Convert.ToDecimal(matched.Groups[1].ToString(), new CultureInfo("en-US"));

            decimal plusIof = System.Math.Round(UsdBrl * 1.0038m, 2, MidpointRounding.AwayFromZero);

            return plusIof;
        }

        public async Task<byte[]> ParseExchange(string uri)
        {
            try
            {
                var httpClient = new HttpClient(); // Error CS0246: The type or namespace name `HttpClient' could not be found. Are you missing an assembly reference? (CS0246)
                return await httpClient.GetByteArrayAsync(uri);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
