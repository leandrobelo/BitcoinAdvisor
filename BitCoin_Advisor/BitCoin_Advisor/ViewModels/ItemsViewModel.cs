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

        public ItemsViewModel()
        {
            Items = new ObservableRangeCollection<Arbitrage>();

            BitCoin_Advisor.Business.ArbitrageLoader arbitrageLoader = new BitCoin_Advisor.Business.ArbitrageLoader();

            Device.BeginInvokeOnMainThread(async () =>
            {
                Items.AddRange(await arbitrageLoader.LoadSources());
            });

            Device.StartTimer(TimeSpan.FromSeconds(120), () =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {

                        Items.Clear();
                        Items.AddRange(await arbitrageLoader.LoadSources());
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        MessagingCenter.Send(new MessagingCenterAlert
                        {
                            Title = "Error loading data.",
                            Message = ex.Message,
                            Cancel = "OK"
                        }, "message");
                    }
                    finally
                    {
                    }
                });

                return true;
            });
        }
    }
}