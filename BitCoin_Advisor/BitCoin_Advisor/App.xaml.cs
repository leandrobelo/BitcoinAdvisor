using BitCoin_Advisor.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BitCoin_Advisor
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SetMainPage();

        }

        public static void SetMainPage()
        {
            Current.MainPage = new NavigationPage(new ItemsPage());
        }
    }
}
