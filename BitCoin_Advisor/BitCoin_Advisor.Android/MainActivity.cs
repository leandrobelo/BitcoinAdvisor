using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace BitCoin_Advisor.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private ArbitrageBinder binder;
        private bool isBound;

        private class ArbitrageServiceConnection : Java.Lang.Object, IServiceConnection
        {
            MainActivity activity;

            public ArbitrageServiceConnection(MainActivity activity)
            {
                this.activity = activity;
            }

            public void OnServiceConnected(ComponentName name, IBinder service)
            {
                var arbitrageBinder = service as ArbitrageBinder;
                if (arbitrageBinder != null)
                {
                    activity.binder = arbitrageBinder;
                    activity.isBound = true;
                }
            }

            public void OnServiceDisconnected(ComponentName name)
            {
                activity.isBound = false;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            BindService(new Intent("BitCoin_Advisor.Droid.ArbitrageService"), new ArbitrageServiceConnection(this), Bind.AutoCreate);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }
    }
}