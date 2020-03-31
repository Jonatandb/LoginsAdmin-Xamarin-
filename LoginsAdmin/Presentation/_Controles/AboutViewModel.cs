using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LoginsAdmin.Presentation._Controles
{
    public class AboutViewModel
    {

        public ICommand AboutCommand { get; }

        public AboutViewModel()
        {

            AboutCommand = new Command(async () =>
            {
                string appVersion = AppInfo.VersionString;
                string action = await Application.Current.MainPage.DisplayActionSheet("LoginsAdmin (" + appVersion + ")", "Aceptar", null, "Email", "Github", "LinkedIn");
                if (!string.IsNullOrEmpty(action))
                {
                    string url;
                    switch (action)
                    {
                        case "Email":
                            url = "mailto:jonatandb@gmail.com?subject=Acerca%20de%20LoginsAdmin%20(" + appVersion + ")&body=Hola%20Jonatandb!,%20";
                            break;
                        case "Github":
                            url = "https://github.com/Jonatandb";
                            break;
                        case "LinkedIn":
                            url = "https://www.linkedin.com/in/jonatandb/";
                            break;
                        default:
                            url = "";
                            break;
                    }
                    if (!string.IsNullOrEmpty(url))
                        if (await Launcher.CanOpenAsync(new Uri(url)))
                            await Launcher.OpenAsync(new Uri(url));
                }
            });
        }
    }
}
