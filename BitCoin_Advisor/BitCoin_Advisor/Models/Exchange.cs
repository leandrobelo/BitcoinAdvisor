using BitCoin_Advisor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitCoin_Advisor
{
    public class Exchange : BaseDataObject
    {
        string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        string image = string.Empty;
        public string Image
        {
            get { return image; }
            set { SetProperty(ref image, value); }
        }

        decimal fee = 0;
        public decimal Fee
        {
            get { return fee; }
            set { SetProperty(ref fee, value); }
        }

        string currency = "";
        public string Currency
        {
            get { return currency; }
            set { SetProperty(ref currency, value); }
        }

        bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetProperty(ref isEnabled, value); }
        }
    }
}
