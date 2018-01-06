using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using BitCoin_Advisor.Business;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BitCoin_Advisor.Droid
{
    [Service(DirectBootAware = true, Exported = true)]
    [IntentFilter(new String[] { "BitCoin_Advisor.Droid.ArbitrageService" })]
    public class ArbitrageService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return new ArbitrageBinder(this);
        }

        public override void OnCreate()
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        await ArbitrageLoader.LoadSources();
                    }
                    finally
                    {
                        Thread.Sleep(5000);
                    }
                }
            });

            base.OnCreate();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            return base.OnStartCommand(intent, flags, startId);
        }
    }
}