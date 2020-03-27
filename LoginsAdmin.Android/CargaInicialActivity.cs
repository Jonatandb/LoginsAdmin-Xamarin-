using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

namespace LoginsAdmin.Droid
{
    [Activity(  MainLauncher = true,
                Label = "LoginsAdmin",
                Icon = "@mipmap/icon",
                Theme = "@style/LoginsAdmin.PantallaDeCargaInicial",
                NoHistory = true, 
                ScreenOrientation = ScreenOrientation.Portrait)]
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