using BitCoin_Advisor.Business;
using BitCoin_Advisor.Helpers;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace BitCoin_Advisor.ViewModels
{

    public class SettingsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Exchange> Items { get; set; }

        public SettingsViewModel()
        {
            Title = "Settings";

            Items = ArbitrageLoader.Exchanges;

        }
    }
}
