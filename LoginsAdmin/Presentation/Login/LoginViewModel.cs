using LoginsAdmin.Domain.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace LoginsAdmin.Presentation.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _clave;
        
        private bool _firstAccess; 

        public event PropertyChangedEventHandler PropertyChanged;


        public LoginViewModel()
        {
            AccederCommand = new Command<Entry>(Acceder, IsAccessButtonEnabled);
            EstablecerClaveCommand = new Command<Entry>(EstablecerClave, IsAccessButtonEnabled);
            VerifyFirstAccess();
        }


        public ICommand AccederCommand { get; set; }

        public ICommand EstablecerClaveCommand { get; set; }

        public string Clave {
            get
            {
                return _clave;
            }
            set
            {
                if (_clave == value) return;
                _clave = value;
                OnPropertyChanged(nameof(Clave));
                ((Command)AccederCommand).ChangeCanExecute();
                ((Command)EstablecerClaveCommand).ChangeCanExecute();
            } 
        }

        public bool ShouldShowFirstAccessControls {
            get => _firstAccess;
            set
            {
                if (_firstAccess == value) return;
                _firstAccess = value;
                OnPropertyChanged(nameof(ShouldShowFirstAccessControls));
            }
        }
        
        public bool ShouldShowAccessControls
        {
            get => !_firstAccess;
        }


        private async void VerifyFirstAccess()
        {
            Usuario usuario = App.RepoServicios.ObtenerUsuarioPrincipal();
            if (usuario == null)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Atención",
                    "Se produjo un error inesperado.\n\nPor favor contactarse con jonatandb@gmail.com\n\nLoginViewModel.VerifyFirstAccess(): " + App.RepoServicios.StatusMessage,
                    "Cerrar");
            }
            else
            {
                if (usuario.Password == "loginsadmindefaultpassword")
                {
                    // Todavía no se estableció una contraseña, solicito que se cree una nueva:
                    usuario.Password = "";
                    _firstAccess = true;
                    OnPropertyChanged(nameof(ShouldShowFirstAccessControls));
                }
            }
        }

        private bool IsAccessButtonEnabled(object obj)
        {
            return !string.IsNullOrWhiteSpace(Clave);
        }

        private async void Acceder(Entry txtPassword)
        {
            try
            {
                if ( ! App.RepoServicios.Login(Clave.Trim()))
                {
                    if (App.RepoServicios.StatusMessage != "")
                    {
                        await App.Current.MainPage.DisplayAlert(
                            "Atención",
                            "Se produjo un error inesperado.\n\nPor favor contactarse con jonatandb@gmail.com\n\nLoginViewModel.Acceder(): " + App.RepoServicios.StatusMessage,
                            "Cerrar");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(
                            "Atención",
                            "Contraseña incorrecta",
                            "Reintentar");
                        EstablecerFoco(txtPassword);
                    }
                }
                else
                {
                    CargarPantallaPrincipal();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Atención",
                    "Se produjo un error inesperado.\n\nPor favor contactarse con jonatandb@gmail.com\n\nLoginViewModel.Acceder(): " + ex.Message,
                    "Aceptar");
            }
        }

        private async void EstablecerClave(Entry txtNewPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(Clave.Trim()))
                {
                    await App.Current.MainPage.DisplayAlert(
                        "Atención",
                        "Se requiere que al menos se especifíque una contraseña.",
                        "Aceptar");
                    EstablecerFoco(txtNewPassword);
                }
                else
                {
                    string nuevaClave = Clave.Trim();
                    if (!App.RepoServicios.EstablecerClaveUsuarioPrincipal(nuevaClave))
                    {
                        await App.Current.MainPage.DisplayAlert(
                            "Atención",
                            "Se produjo un error al establecer la contraseña.\n\nPor favor contactarse con jonatandb@gmail.com\n\nLoginViewModel.EstablecerClave(): " + App.RepoServicios.StatusMessage,
                            "Cerrar");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(
                            "LoginsAdmin",
                            "Contraseña: '" + nuevaClave + "', establecida exitosamente!",
                            "Continuar");
                        CargarPantallaPrincipal();
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Atención",
                    "Se produjo un error inesperado.\n\nPor favor contactarse con jonatandb@gmail.com\n\nLoginViewModel.EstablecerClave(): " + ex.Message,
                    "Aceptar");
            }
        }

        private void EstablecerFoco(Entry txt)
        {
            txt.CursorPosition = 0;
            txt.SelectionLength = txt.Text.Length;
            txt.Focus();
        }
        private async void CargarPantallaPrincipal()
        {
            App.IsUserLoggedIn = true;
            ContentPage mainPage = new Inicio();
            NavigationPage.SetHasBackButton(mainPage, false);
            NavigationPage.SetHasNavigationBar(mainPage, false);
            App.Current.MainPage.Navigation.InsertPageBefore(mainPage, App.Current.MainPage.Navigation.NavigationStack[0]);
            await App.Current.MainPage.Navigation.PopAsync();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(propertyName)));
        }
    }
}
