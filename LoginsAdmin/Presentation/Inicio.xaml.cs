using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoginsAdmin.Presentation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Inicio : ContentPage
    {
      //  public ObservableCollection<string> Items { get; set; }

        public Inicio()
        {
            InitializeComponent();
        }

        private async void btnAgregarServicio_Clicked(object sender, EventArgs e)
        {
            if(await this.DisplayAlert(
                "Agregar Servicio",
                "¿Está seguro que desea agregar un nuevo servicio?",
                "Agregar",
                "Cancelar"))
            {
                this.Content = new Detalles();
            }
        }

    }
}
