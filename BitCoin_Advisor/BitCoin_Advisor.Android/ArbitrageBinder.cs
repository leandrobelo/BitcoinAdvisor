using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BitCoin_Advisor.Droid
{
    public class ArbitrageBinder : Binder
    {
        public ArbitrageBinder(ArbitrageService service)
        {
            Service = service;
        }

        public ArbitrageService Service { get; }
    }
}