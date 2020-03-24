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

        private void btnAgregarServicio_Clicked(object sender, EventArgs e)
        {
            this.Content = new Detalles();
        }

    }
}
