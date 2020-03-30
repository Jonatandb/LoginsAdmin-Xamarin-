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
                string action = await Application.Current.MainPage.DisplayActionSheet("LoginsAdmin by Jonatandb", "Aceptar", null, "Email", "Github", "LinkedIn");
                if (!string.IsNullOrEmpty(action))
                {
                    string url;
                    switch (action)
                    {
                        case "Email":
                            url = "mailto:jonatandb@gmail.com";
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
