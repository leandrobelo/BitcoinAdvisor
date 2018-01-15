using BitCoin_Advisor.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitCoin_Advisor.Models
{
    public class Arbitrage : BaseDataObject
    {
        public Arbitrage(ExchangeTicker from, ExchangeTicker to)
        {
            this.From = from;
            this.To = to;

            this.ExchangeRateLabel = string.Format("{0}/{1}:", this.From.Exchange.Currency, this.To.Exchange.Currency);
        }

        ExchangeTicker from = null;
        public ExchangeTicker From
        {
            get { return from; }
            set { SetProperty(ref from, value); }
        }

        ExchangeTicker to = null;
        public ExchangeTicker To
        {
            get { return to; }
            set { SetProperty(ref to, value); }
        }

        decimal conversion = 0;
        public decimal Conversion
        {
            get { return conversion; }
            set { SetProperty(ref conversion, value); }
        }

        decimal profit = 0;
        public decimal Profit
        {
            get
            {
                return profit;
            }
            set
            {
                SetProperty(ref profit, value);
            }
        }

        public void CalculateNet()
        {
            decimal currentBitcoin = (from.Price.Last * from.Exchange.Fee) / from.Price.Last;
            decimal netTo = (currentBitcoin * to.Price.Last * to.Exchange.Fee) / conversion;

            Profit = (netTo - from.Price.Last) / from.Price.Last;
        }


        string exchangeRateLabel = "";
        public string ExchangeRateLabel
        {
            get { return exchangeRateLabel; }
            set
            {
                SetProperty(ref exchangeRateLabel, value);
            }
        }
    }
}
