using LoginsAdmin.Domain.Models;
using LoginsAdmin.Presentation.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoginsAdmin.Presentation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Inicio : ContentPage
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void GrillaServicios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((InicioViewModel)BindingContext).SelectedService = (Servicio)e.Item;
            ((InicioViewModel)BindingContext).EditServiceCommand.Execute(null);
        }
    }
}