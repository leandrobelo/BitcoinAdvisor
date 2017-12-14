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
        public Arbitrage(Source from, Source to)
        {
            this.From = from;
            this.To = to;

            this.ExchangeRateLabel = string.Format("{0}/{1}:", this.From.Currency, this.To.Currency);
        }


        Source from = null;
        public Source From
        {
            get { return from; }
            set { SetProperty(ref from, value); }
        }

        Source to = null;
        public Source To
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
                CalculateNet();
                return profit;
            }
            set
            {
                SetProperty(ref profit, value);
            }
        }

        public void CalculateNet()
        {
             decimal currentBitcoin = (from.Price.Last * from.Fee) / from.Price.Last;
            decimal netTo = (currentBitcoin * to.Price.Last * to.Fee) / conversion;


            profit = (netTo - from.Price.Last) / from.Price.Last;
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
