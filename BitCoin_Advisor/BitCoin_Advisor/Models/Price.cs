using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitCoin_Advisor.Models
{
    public class Ticker
    {
        [JsonProperty(PropertyName = "last", Required = Required.Always)]
        public decimal Last { get; set; }
    }

    public class Tickers
    {
        [JsonProperty("ticker")]
        public Ticker ticker { get; set; }
    }
}
