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
        public Arbitrage(Source from, Source to, decimal capital, decimal conversion)
        {
            this.From = from;
            this.To = to;
            this.Capital = capital;

            this.ExchangeRateLabel = string.Format("{0}/{1}:", this.From.Currency, this.To.Currency);
            this.Conversion = conversion;
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

        decimal netAmount = 0;
        public decimal NetAmount
        {
            get
            {
                CalculateNet();
                return netAmount;
            }
            set
            {
                SetProperty(ref netAmount, value);
            }
        }

        public void CalculateNet()
        {
            netAmount = (((Capital * from.Fee) / From.Price.Last) * to.Price.Last) * to.Fee - capital * conversion;
        }

        decimal capital = 0;
        public decimal Capital
        {
            get { return capital; }
            set
            {
                SetProperty(ref capital, value);

                CalculateNet();
                SetProperty(ref netAmount, value);
            }
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
