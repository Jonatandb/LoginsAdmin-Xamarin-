using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Java.IO;

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
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            // Tests de exportación de datos:
            File downloadsPath = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads);
            DownloadManager dm = (DownloadManager)this.GetSystemService(DownloadService);
            dm.AddCompletedDownload("titulo", "description", false, "text/plain", downloadsPath.ToString(), 10, true);

            LoadApplication(new App(downloadsPath.ToString()));

            // Archivos modificados:
            /* AndroidManifest  -> Agrego solicitud de permisos de lectura y escritura en la SD
             * App.xaml.cs      -> Llamo a FileAccessHelper.ExportDataAsync(downloadsPath).Wait();
             * FileAccessHelper -> Agrego método ExportDataAsync
             * Inicio.xaml y cs -> Agrego botón
             * 
             * Páginas de referencia:
             *  Xamarin.Essentials: Asistentes del sistema de archivos  https://docs.microsoft.com/es-es/xamarin/essentials/file-system-helpers?context=xamarin%2Fandroid&tabs=android
             *  Xamarin.Essentials: Almacenamiento seguro               https://docs.microsoft.com/es-es/xamarin/essentials/secure-storage?context=xamarin%2Fandroid&tabs=android
             *  Almacenamiento y acceso a archivos con Xamarin.Android  https://docs.microsoft.com/es-es/xamarin/android/platform/files/
             *  Almacenamiento externo                                  https://docs.microsoft.com/es-es/xamarin/android/platform/files/external-storage?tabs=windows
             *  How to correctly save and read files                    https://forums.xamarin.com/discussion/6990/how-to-correctly-save-and-read-files
             *  Cómo guardar un archivo en almacenamiento externo       https://developer.android.com/training/data-storage/files/external#PublicFiles
             *  Cómo guardar archivos en un dispositivo de almacenamiento   https://developer.android.com/training/data-storage/files
             *  How to store files generated from app in “Downloads” folder of Android? https://stackoverflow.com/questions/28183893/how-to-store-files-generated-from-app-in-downloads-folder-of-android
             *  Android: Save local File to Downloads Folder and make it visible        https://stackoverflow.com/questions/33872592/android-save-local-file-to-downloads-folder-and-make-it-visible
             *  Cómo crear un documento nuevo                           https://developer.android.com/guide/topics/providers/document-provider#create
             *  Cómo crear una copia de seguridad automática para los datos del usuario https://developer.android.com/guide/topics/data/autobackup
             *  SQLite DB to SD Demo                                    https://www.techrepublic.com/blog/software-engineer/export-sqlite-data-from-your-android-device/
             */

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}