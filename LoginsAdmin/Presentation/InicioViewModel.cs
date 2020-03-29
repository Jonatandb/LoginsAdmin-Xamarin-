using Xamarin.Forms;
using System.ComponentModel;
using LoginsAdmin.Domain.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LoginsAdmin.Presentation.ViewModels
{
    public class InicioViewModel : INotifyPropertyChanged
    {
        string searchText;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AddServiceCommand { get; }
        public ICommand EditServiceCommand { get; }
        public ICommand DeleteServiceCommand { get; }
        public ObservableCollection<Servicio> Servicios { get; }

        public InicioViewModel()
        {
            Servicios = new ObservableCollection<Servicio>();

            AddServiceCommand = new Command(() =>
            {
                ContentPage ABM = new ABM();
                ((ABMViewModel)ABM.BindingContext).ServicesModified += RecargarGrilla;
                NavigationPage.SetHasBackButton(ABM, false);
                NavigationPage.SetHasNavigationBar(ABM, false);
                Application.Current.MainPage.Navigation.PushAsync(ABM);
            });

            EditServiceCommand = new Command(service =>
            {
                ContentPage ABM = new ABM();
                ((ABMViewModel)ABM.BindingContext).SetServiceToEdit((Servicio) service);
                ((ABMViewModel)ABM.BindingContext).ServicesModified += RecargarGrilla;
                NavigationPage.SetHasBackButton(ABM, false);
                NavigationPage.SetHasNavigationBar(ABM, false);
                Application.Current.MainPage.Navigation.PushAsync(ABM);
            });

            DeleteServiceCommand = new Command( (serviceId) =>
            {
                ABMViewModel abmVM = new ABMViewModel();
                abmVM.ServicesModified += RecargarGrilla;
                abmVM.EliminarCommand.Execute(serviceId);
            });
        }

        public void RecargarGrilla(object sender = null, object e = null)
        {
            Servicios.Clear();
            App.RepoServicios.ObtenerServicios(SearchText).ForEach(s => Servicios.Add(s));
            var args2 = new PropertyChangedEventArgs(nameof(ResultSearchText));
            PropertyChanged(nameof(ResultSearchText), args2);
        }

        public string SearchText
        {
            get => string.IsNullOrWhiteSpace(searchText) ? "" : searchText;
            set
            {
                searchText = value;

                var args = new PropertyChangedEventArgs(nameof(SearchText));
                PropertyChanged?.Invoke(this, args);
                RecargarGrilla();
            }
        }

        public string ResultSearchText { 
            get 
            {
                // Texto de búsqueda    Servicios       Mensaje de estado
                // ""                   0               Aún no se agregaron servicios...
                // ""                   1               Total de servicios: 1
                // xxx                  0               Total de servicios que incluyen 'xxx': 0
                // xxx                  1               Total de servicios que incluyen 'xxx': 1
                
                if (string.IsNullOrWhiteSpace(SearchText) && Servicios.Count == 0)
                    return "Aún no se agregaron servicios...";
                else if (string.IsNullOrWhiteSpace(SearchText) && Servicios.Count != 0)
                    return "Total de servicios: " + Servicios.Count;
                else
                    if (Servicios.Count == 0)
                    {
                        return "Ningún resultado";              // ¿Agrego texto clickeable que diga "Click aquí para crear el servicio + SearchText"? ************
                    }
                    else 
                    { 
                        return "Total de servicios que incluyen '" + ((SearchText.Trim().Length > 10) ? SearchText.Trim().Substring(0, 10) + "..." : SearchText.Trim()) + "': " + Servicios.Count;
                    }
            }
        }

    }
}
