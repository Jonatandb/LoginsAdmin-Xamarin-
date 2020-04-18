using LoginsAdmin.Utils;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LoginsAdmin.Presentation.ViewModels
{
    public class AboutViewModel
    {

        public AboutViewModel()
        {
            AboutCommand = new Command(About);
        }

        public ICommand AboutCommand { get; }

        public async void About()
        {
            string appVersion = "(" + AppInfo.VersionString + ")";

            string action = string.Empty;

            Page currentPage = null;
            if (Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
            {
                int index = Application.Current.MainPage.Navigation.NavigationStack.Count - 1;
                currentPage = Application.Current.MainPage.Navigation.NavigationStack[index];
            }

            if(currentPage != null && currentPage.ToString() == "LoginsAdmin.Presentation.Inicio")
            {
                action = await Application.Current.MainPage.DisplayActionSheet("LoginsAdmin " + appVersion, "Aceptar", null, "Email", "Github", "LinkedIn", "Exportar Datos", "Importar Datos");
            }            
            else
            {
                action = string.Empty; await Application.Current.MainPage.DisplayActionSheet("LoginsAdmin " + appVersion, "Aceptar", null, "Email", "Github", "LinkedIn");
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
                        if (ImportExportDataHelper.ImportData())
                        {
                            await Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                "Importación finalizada.\n\n" + ImportExportDataHelper.StatusMessage,
                                "Cerrar");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                "Falló la importación.\n\n" + ImportExportDataHelper.StatusMessage,
                                "Cerrar");
                        }
                        break;
                    case "Exportar Datos":
                        if (ImportExportDataHelper.ExportData())
                        {
                            await Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                "Exportación finalizada.\n\n" + ImportExportDataHelper.StatusMessage,
                                "Cerrar");
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                "Falló la exportación.\n\n" + ImportExportDataHelper.StatusMessage,
                                "Cerrar");
                        }
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

    }
}
