using BitCoin_Advisor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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

        private bool? isEnabled;
        public bool IsEnabled
        {
            get
            {
                if (!isEnabled.HasValue)
                {
                    var propertyKey = String.Format("Exchange_Enabled_{0}", this.name);
                    if (!Application.Current.Properties.ContainsKey(propertyKey) ||
                        !Boolean.TryParse(Convert.ToString(Application.Current.Properties[propertyKey]), out bool propertyValue))
                        propertyValue = true;

                    isEnabled = propertyValue;
                }
                return isEnabled.Value;
            }
            set
            {
                SetProperty(ref isEnabled, value);
                Application.Current.Properties[String.Format("Exchange_Enabled_{0}", this.name)] = value;
            }
        }
    }
}
