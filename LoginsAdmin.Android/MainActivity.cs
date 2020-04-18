using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.IO;

namespace LoginsAdmin.Droid
{
    [Activity(  Label = "LoginsAdmin", 
                Icon = "@mipmap/icon", 
                Theme = "@style/MainTheme", 
                ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, 
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            ConfigureBackupFilesPath();

            LoadApplication(new App());
        }

        private static void ConfigureBackupFilesPath()
        {
            string downloadsDiretory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
            string backupFilePath = Path.Combine(downloadsDiretory, "Datos_LoginsAdmin.csv");
            App.BackupFilePath = backupFilePath;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}