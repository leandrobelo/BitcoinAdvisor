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
            Current.MainPage = new TabbedPage
            {
                BarBackgroundColor = Color.FromHex("#00548C"),
                BackgroundColor = Color.FromHex("#3F3F46"),
                Children =
                {
                    new NavigationPage(new ItemsPage())
                    {
                        Title = "Browse",
                        Icon = Device.OnPlatform("tab_feed.png",null,null)
                    },
                    new NavigationPage(new AboutPage())
                    {
                        Title = "About",
                        Icon = Device.OnPlatform("tab_about.png",null,null)
                    },
                }
            };
        }
    }
}
