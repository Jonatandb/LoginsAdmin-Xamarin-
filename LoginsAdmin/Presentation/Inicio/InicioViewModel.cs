using Xamarin.Forms;
using System.ComponentModel;
using LoginsAdmin.Domain.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LoginsAdmin.Presentation.ViewModels
{
    public class InicioViewModel : INotifyPropertyChanged
    {
        string _searchText;

        public event PropertyChangedEventHandler PropertyChanged;
        
        
        public InicioViewModel()
        {
            Servicios = new ObservableCollection<Servicio>();
            AddEditServiceCommand = new Command(ShowABM);
        }


        public ICommand AddEditServiceCommand { get; }

        public ObservableCollection<Servicio> Servicios { get; }
        
        public Servicio SelectedService { get; set; }
        
        public string SearchText
        {
            get => string.IsNullOrWhiteSpace(_searchText) ? "" : _searchText;
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
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
                        return "Ningún resultado";
                    }
                    else 
                    { 
                        return "Total de servicios que incluyen '" + ((SearchText.Trim().Length > 10) ? SearchText.Trim().Substring(0, 10) + "..." : SearchText.Trim()) + "': " + Servicios.Count;
                    }
            }
        }


        public void RecargarGrilla()
        {
            SelectedService = null;
            Servicios.Clear();
            App.RepoServicios.ObtenerServicios(SearchText).ForEach(s => Servicios.Add(s));
            OnPropertyChanged(nameof(ResultSearchText));
        }
        
        private async void ShowABM()
        {
            ContentPage ABM = new ABM();
            ((ABMViewModel)ABM.BindingContext).SetServiceToEdit(SelectedService);
            ((ABMViewModel)ABM.BindingContext).ServicesModified += RecargarGrilla;
            NavigationPage.SetHasBackButton(ABM, false);
            NavigationPage.SetHasNavigationBar(ABM, false);
            await Application.Current.MainPage.Navigation.PushAsync(ABM);
            SelectedService = null;
        }
        
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
