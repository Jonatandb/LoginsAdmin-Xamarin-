using LoginsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.IsUserLoggedIn = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.stackCrearClave.IsVisible = false;
            this.stackIngresarClave.IsVisible = false;
            //Usuario usuario = await App.RepoServicios.ObtenerUsuarioPrincipal();
            Usuario usuario = App.RepoServicios.ObtenerUsuarioPrincipal();
            if(usuario == null)
            {
                this.DisplayAlert(
                                "Atención",
                                "Se produjo un error inesperado.\n\nPor favor contactarse con jonatandb@gmail.com\n\n" + App.RepoServicios.StatusMessage,
                                "Cerrar");
            }
            else
            {
                if(usuario.Password == "loginsadmindefaultpassword")
                {
                    // Todavía no se estableció una contraseña, por lo que solicito que se cree una nueva:

                    usuario.Password = "";
                    this.stackCrearClave.IsVisible = true;
                }
                else
                {
                    this.stackIngresarClave.IsVisible = true;
                }
            }
        }

        private async void btnEstablecerPassword_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNewPassword.Text))
            {
                // No se ingresó una clave válida

                await this.DisplayAlert(
                                    "Atención",
                                    "Se requiere que al menos se especifíque una contraseña.",
                                    "Aceptar");
                this.txtNewPassword.Focus();
            }
            else
            {
                // Guardo la nueva clave

                string nuevaClave = this.txtNewPassword.Text.Trim();
                //if (! await App.RepoServicios.EstablecerClaveUsuarioPrincipal(nuevaClave))
                if ( ! App.RepoServicios.EstablecerClaveUsuarioPrincipal(nuevaClave))
                {
                    // No se pudo actualizar el registro

                    await this.DisplayAlert(
                        "Atención",
                        "Se produjo un error al establecer la contraseña.\n\nPor favor contactarse con jonatandb@gmail.com\n\n" + App.RepoServicios.StatusMessage,
                        "Cerrar");
                }
                else
                {
                    // Clave actualizada exitosamente

                    await this.DisplayAlert(
                        "LoginsAdmin",
                        "Contraseña: '" + nuevaClave + "', establecida exitosamente!",
                        "Continuar");

                    PantallaPrincipal();
                }
            }
        }

        private void txtNewPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            // El botón de Crear contraseña solo aparece habilitado si se ingresa texto

            this.btnEstablecerPassword.IsEnabled = !string.IsNullOrEmpty(e.NewTextValue.Trim());
        }

        private async void btnAcceder_Clicked(object sender, EventArgs e)
        {
            if (!App.RepoServicios.Login(this.txtPassword.Text.Trim()))
            {
                if(App.RepoServicios.StatusMessage != "")
                {
                    await this.DisplayAlert(
                        "Atención",
                        "Se produjo un error al iniciar sesión.\n\nPor favor contactarse con jonatandb@gmail.com\n\nDetalles del error: \n" + App.RepoServicios.StatusMessage,
                        "Cerrar");
                }   
                else
                {
                    // Error al iniciar sesión

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
                PantallaPrincipal();
            }
        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            // El botón de Acceder solo aparece habilitado si se ingresa texto

            this.btnAcceder.IsEnabled = !string.IsNullOrEmpty(e.NewTextValue.Trim());

        }

        /// <summary>
        /// Carga de la pantalla principal
        /// </summary>
        private void PantallaPrincipal()
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