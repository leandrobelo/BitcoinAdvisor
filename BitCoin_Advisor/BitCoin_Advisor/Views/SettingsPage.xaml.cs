
using BitCoin_Advisor.ViewModels;
using Xamarin.Forms;

namespace BitCoin_Advisor.Views
{
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel viewModel;

        public SettingsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SettingsViewModel();
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var list = (ListView)sender;
            list.SelectedItem = null;
        }
        
    }
}
