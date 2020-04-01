using LoginsAdmin.Domain.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoginsAdmin.Presentation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Usuario usuario = App.RepoServicios.ObtenerUsuarioPrincipal();
            if(usuario == null)
            {
                this.DisplayAlert(
                    "Atención",
                    "Se produjo un error inesperado.\n\nPor favor contactarse con jonatandb@gmail.com\n\nLogin.OnAppearing(): " + App.RepoServicios.StatusMessage,
                    "Cerrar");
            }
            else
            {
                if(usuario.Password == "loginsadmindefaultpassword")
                {
                    // Todavía no se estableció una contraseña, solicito que se cree una nueva:

                    usuario.Password = "";
                    this.stackCrearClave.IsVisible = true;
                }
                else
                {
                    this.stackIngresarClave.IsVisible = true;
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.IsUserLoggedIn = false;
        }

        private async void btnEstablecerPassword_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtNewPassword.Text))
                {
                    await this.DisplayAlert(
                        "Atención",
                        "Se requiere que al menos se especifíque una contraseña.",
                        "Aceptar");
                    this.txtNewPassword.Focus();
                }
                else
                {
                    string nuevaClave = this.txtNewPassword.Text.Trim();
                    if ( ! App.RepoServicios.EstablecerClaveUsuarioPrincipal(nuevaClave))
                    {
                        await this.DisplayAlert(
                            "Atención",
                            "Se produjo un error al establecer la contraseña.\n\nPor favor contactarse con jonatandb@gmail.com\n\nLogin.btnEstablecerPassword_Clicked(): " + App.RepoServicios.StatusMessage,
                            "Cerrar");
                    }
                    else
                    {
                        await this.DisplayAlert(
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
                    "Se produjo un error inesperado.\n\nPor favor contactarse con jonatandb@gmail.com\n\nLogin.btnEstablecerPassword_Clicked(): " + ex.Message,
                    "Aceptar");
            }
        }

        private async void btnAcceder_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!App.RepoServicios.Login(this.txtPassword.Text.Trim()))
                {
                    if(App.RepoServicios.StatusMessage != "")
                    {
                        await this.DisplayAlert(
                            "Atención",
                            "Se produjo un error inesperado.\n\nPor favor contactarse con jonatandb@gmail.com\n\nLogin.btnAcceder_Clicked(): " + App.RepoServicios.StatusMessage,
                            "Cerrar");
                    }   
                    else
                    {
                        await this.DisplayAlert(
                            "Atención",
                            "Contraseña incorrecta",
                            "Reintentar");
                        this.txtPassword.Text = "";
                        this.txtPassword.Focus();
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
                    "Se produjo un error inesperado.\n\nPor favor contactarse con jonatandb@gmail.com\n\nLogin.btnAcceder_Clicked(): " + ex.Message,
                    "Aceptar");
            }
        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.btnAcceder.IsEnabled = !string.IsNullOrEmpty(e.NewTextValue.Trim());
        }

        private void txtNewPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.btnEstablecerPassword.IsEnabled = !string.IsNullOrEmpty(e.NewTextValue.Trim());
        }

        private void CargarPantallaPrincipal()
        {
            App.IsUserLoggedIn = true;
            ContentPage mainPage = new Inicio();
            NavigationPage.SetHasBackButton(mainPage, false);
            NavigationPage.SetHasNavigationBar(mainPage, false);
            Navigation.InsertPageBefore(mainPage, this);
            Navigation.PopAsync();
        }
    }
}