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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.IsUserLoggedIn = false;
        }

    }
}