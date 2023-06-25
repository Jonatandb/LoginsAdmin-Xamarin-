using Xamarin.Forms;
using LoginsAdmin.Domain.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;

namespace LoginsAdmin.Presentation.ViewModels
{
    public class InicioViewModel : BaseViewModel
    {
        string _searchText;
        private bool _isBusy;


        public InicioViewModel()
        {
            Servicios = new ObservableCollection<Servicio>();
            AddEditServiceCommand = new Command<Servicio>(ShowABM);
            MessagingCenter.Subscribe<BaseViewModel>(this, "ServicesModified", (e) => RecargarGrilla());
            MessagingCenter.Subscribe<BaseViewModel, bool>(this, "Processing", (vm, value) => {
                IsBusy = value;
            });
        }


        public ICommand AddEditServiceCommand { get; }

        public ObservableCollection<Servicio> Servicios { get; }
        
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
                
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    if(Servicios.Count == 0)
                    {
                        return "Aún no se agregaron servicios...";
                    } 
                    else
                    {
                        return "Total de servicios: " + Servicios.Count;
                    }
                }
                else
                {
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
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                    
                    // Bloqueo todos los controles de la pantalla mientras 
                    // se está llevando a cabo alguna operación que demora
                    var currentPage = Application.Current.MainPage;
                    currentPage.IsEnabled = !IsBusy;
                }
            }
        }


        public void RecargarGrilla()
        {
            Servicios.Clear();
            App.RepoServicios.ObtenerServicios(SearchText).ForEach(s => Servicios.Add(s));
            OnPropertyChanged(nameof(ResultSearchText));
        }
        
        private async void ShowABM(Servicio servicio)
        {
            App.SelectedService = servicio;
            ContentPage ABM = new ABM();
            await Application.Current.MainPage.Navigation.PushAsync(ABM, false);
        }

    }
}
