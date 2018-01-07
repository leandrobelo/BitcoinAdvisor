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
        public ObservableRangeCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemDetailViewModel(Item item = null)
        {
            Title = "Arbitrage";
            Items = new ObservableRangeCollection<Item>();
            var task = ExecuteLoadItemsCommand();
        }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }


        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                //Items.Clear();
                //var items = await DataStore.GetItemsAsync(true);
                //Items.Add(items);
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

            Device.StartTimer(TimeSpan.FromSeconds(15), () =>
            {

                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        if (IsBusy)
                            return;

                        IsBusy = true;

                        //var items = await DataStore.GetItemsAsync(true);
                        /*
                        if (Items.Count == 0 || Items.First().Valor != items.Valor)
                        {
                            Items.Insert(0, items);
                        }
                        */
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
                        IsBusy = false;
                    }
                });

                return true;
            });
        }
    }
}