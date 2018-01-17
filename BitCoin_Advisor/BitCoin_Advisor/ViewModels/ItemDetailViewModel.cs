using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BitCoin_Advisor.Helpers;
using BitCoin_Advisor.Models;
using BitCoin_Advisor.Views;
using System.Linq;
using Xamarin.Forms;

namespace BitCoin_Advisor.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Arbitrage Item { get; set; }

        public ItemDetailViewModel(Arbitrage item = null)
        {
            Title = "Arbitrage";
            this.Item = item;
        }
    }
}