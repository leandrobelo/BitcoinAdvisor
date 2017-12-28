using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitCoin_Advisor.Models
{
    public class ExchangeTicker : BaseDataObject
    {
        Exchange exchange = null;
        public Exchange Exchange
        {
            get { return exchange; }
            set { SetProperty(ref exchange, value); }
        }

        Symbol symbol = null;
        public Symbol Symbol
        {
            get { return symbol; }
            set { SetProperty(ref symbol, value); }
        }

        string tickerurl = string.Empty;
        public string TickerUrl
        {
            get { return tickerurl; }
            set { SetProperty(ref tickerurl, value); }
        }

        Ticker price = null;
        public Ticker Price
        {
            get { return price; }
            set { SetProperty(ref price, value); }
        }
    }
}
