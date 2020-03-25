using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace LoginsAdmin.Droid
{
    [Activity(Theme = "@style/LoginsAdmin.PantallaDeCargaInicial", MainLauncher = true, NoHistory = true, Label = "LoginsAdmin", Icon = "@mipmap/icon" )]
    public class CargaInicialActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { StartMainActivityOnBackground(); });
            startupWork.Start();
        }

        void StartMainActivityOnBackground()
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        public override void OnBackPressed() { }

    }
}