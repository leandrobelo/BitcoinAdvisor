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
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Arbitrage> Items { get; set; }

        public Command LoadItemsCommand
        {
            get
            {
                return new Command(async () =>
                {
                    //IsRefreshing = true;

                    await ExecuteLoadItemsCommand();

                    //IsRefreshing = false;
                });
            }
        }

        BitCoin_Advisor.Business.ArbitrageLoader arbitrageLoader = new BitCoin_Advisor.Business.ArbitrageLoader();

        public ItemsViewModel()
        {
            Items = new ObservableRangeCollection<Arbitrage>();

            ExecuteLoadItemsCommand();
        }


        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();

                Device.BeginInvokeOnMainThread(async () =>
                {
                    Items.AddRange(await arbitrageLoader.LoadSources());
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}