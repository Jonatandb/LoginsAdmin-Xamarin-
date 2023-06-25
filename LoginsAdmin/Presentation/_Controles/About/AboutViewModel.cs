using LoginsAdmin.Utils;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LoginsAdmin.Presentation.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {

        public AboutViewModel()
        {
            AboutCommand = new Command(MostrarMenu);
        }

        public ICommand AboutCommand { get; }

        public async void MostrarMenu()
        {
            string appVersion = "(" + AppInfo.VersionString + ")";

            string action;

            Page currentPage = null;
            if (Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
            {
                int index = Application.Current.MainPage.Navigation.NavigationStack.Count - 1;
                currentPage = Application.Current.MainPage.Navigation.NavigationStack[index];
            }

            if(currentPage != null && currentPage.ToString() == "LoginsAdmin.Presentation.Inicio")
            {
                if (VerifyBackupFileExists())
                {
                    action = await Application.Current.MainPage.DisplayActionSheet("LoginsAdmin " + appVersion, "Aceptar", null, "Email", "Github", "LinkedIn", "Exportar Datos", "Enviar archivo de datos por mail", "Importar Datos");
                } else
                {
                    action = await Application.Current.MainPage.DisplayActionSheet("LoginsAdmin " + appVersion, "Aceptar", null, "Email", "Github", "LinkedIn", "Exportar Datos", "Importar Datos");
                }
            }            
            else
            {
                action = await Application.Current.MainPage.DisplayActionSheet("LoginsAdmin " + appVersion, "Aceptar", null, "Email", "Github", "LinkedIn");
            }

            if (!string.IsNullOrEmpty(action))
            {
                switch (action)
                {
                    case "Email":
                        OpenURL("mailto:jonatandb@gmail.com?subject=Acerca%20de%20LoginsAdmin%20" + appVersion + "&body=Hola%20Jonatandb,%20");
                        break;
                    case "Github":
                        OpenURL("https://github.com/Jonatandb");
                        break;
                    case "LinkedIn":
                        OpenURL("https://www.linkedin.com/in/jonatandb/");
                        break;
                    case "Importar Datos":
                        MessagingCenter.Send((BaseViewModel)this, "Processing", true);
                        if (ImportExportDataHelper.ImportData())
                        {
                            await Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                "Importación finalizada.\n\n" + ImportExportDataHelper.StatusMessage,
                                "Cerrar");
                            MessagingCenter.Send((BaseViewModel)this, "ServicesModified");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                "Falló la importación.\n\n" + ImportExportDataHelper.StatusMessage,
                                "Cerrar");
                        }
                        MessagingCenter.Send((BaseViewModel)this, "Processing", false);
                        break;
                    case "Exportar Datos":
                        MessagingCenter.Send((BaseViewModel)this, "Processing", true);
                        if (ImportExportDataHelper.ExportData())
                        {
                            await Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                "Exportación finalizada!\n\nSe creó el archivo:\n" + ImportExportDataHelper.StatusMessage + "\n\n* Recuerde que este archivo tiene la contraseña de esta aplicación.",
                                "Cerrar");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                "Falló la exportación.\n\n" + ImportExportDataHelper.StatusMessage,
                                "Cerrar");
                        }
                        MessagingCenter.Send((BaseViewModel)this, "Processing", false);
                        break;
                    case "Enviar archivo de datos por mail":
                            await CompartirArchivoAdjunto();
                        break;

                    default:
                        break;
                }
            }
        }

        private async void OpenURL(string url)
        {
            if (!string.IsNullOrEmpty(url))
                if (await Launcher.CanOpenAsync(new Uri(url)))
                    await Launcher.OpenAsync(new Uri(url));
        }

        private async Task CompartirArchivoAdjunto()
        {
            try
            {
                var file = new ShareFile(ImportExportDataHelper.GetBackupFilePath());
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Datos LoginsAdmin",
                    File = file
                });
            }
            catch{}
        }

        private bool VerifyBackupFileExists()
        {
            try
            {
                return new FileInfo(ImportExportDataHelper.GetBackupFilePath()).Exists;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
