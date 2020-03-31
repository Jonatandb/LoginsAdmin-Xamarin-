using Xamarin.Forms;
using System.ComponentModel;
using LoginsAdmin.Domain.Models;
using System.Windows.Input;
using System.Threading.Tasks;

namespace LoginsAdmin.Presentation.ViewModels
{
    public class ABMViewModel : INotifyPropertyChanged
    {
        int _id;
        string _nombre, _usuario, _clave, _otrosDatos;

        public delegate void ServicesModifiedEventHandler();
        public event ServicesModifiedEventHandler ServicesModified;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GuardarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }


        public ABMViewModel()
        {
            GuardarCommand = new Command(async () =>
            {
                if (string.IsNullOrWhiteSpace(Nombre))
                {
                    await App.Current.MainPage.DisplayAlert("Atención",
                                                      "Se requiere que al menos se especifíque un nombre para el servicio.",
                                                      "Aceptar");
                }
                else
                {
                    Servicio servicio = new Servicio
                    {
                        Id = IsEditMode ? ServiceToEdit.Id : -1,
                        Name = Nombre.Trim(),
                        User = Usuario.Trim(),
                        Password = Clave.Trim(),
                        ExtraData = OtrosDatos.Trim()
                    };

                    if (App.RepoServicios.AgregarEditarServicio(servicio))
                    {
                        await Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                                                  App.RepoServicios.StatusMessage,
                                                                  "Cerrar");
                        await Volver();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                                                  App.RepoServicios.StatusMessage,
                                                                  "Cerrar");
                    }
                }
            });

            EliminarCommand = new Command( async () =>
            {
                bool respuesta = await Application.Current.MainPage.DisplayAlert("Atención", "¿Está seguro que desea eliminar este servicio?", "Si", "No");
                if(respuesta)
                {
                    if (IsEditMode)
                    {
                        if (App.RepoServicios.EliminarServicio(ServiceToEdit.Id))
                        {
                            await Volver();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                                                        App.RepoServicios.StatusMessage,
                                                                        "Cerrar");
                        }
                    }
                }
            });
        }


        private Servicio ServiceToEdit { get; set; }
        
        public bool IsEditMode { get => ServiceToEdit != null; }
        
        public bool IsValidServiceName { get => !string.IsNullOrWhiteSpace(Nombre); }

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
            }
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (_nombre == value) return;
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
                OnPropertyChanged(nameof(IsValidServiceName));
            }
        }

        public string Usuario
        {
            get => string.IsNullOrWhiteSpace(_usuario) ? "" : _usuario;
            set
            {
                if (_usuario == value) return;
                _usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        public string Clave
        {
            get => string.IsNullOrWhiteSpace(_clave) ? "" : _clave;
            set
            {
                if (_clave == value) return;
                _clave = value;
                OnPropertyChanged(nameof(Clave));
            }
        }

        public string OtrosDatos
        {
            get => string.IsNullOrWhiteSpace(_otrosDatos) ? "" : _otrosDatos;
            set
            {
                if (_otrosDatos == value) return;
                _otrosDatos = value;
                OnPropertyChanged(nameof(OtrosDatos));
            }
        }


        public void SetServiceToEdit(Servicio service)
        {
            if (service != null)
            {
                ServiceToEdit = service;
                Id = ServiceToEdit.Id;
                Nombre = ServiceToEdit.Name;
                Usuario = ServiceToEdit.User;
                Clave = ServiceToEdit.Password;
                OtrosDatos = ServiceToEdit.ExtraData;
                OnPropertyChanged(nameof(IsEditMode));
            }
        }

        private void RefrescarGrilla()
        {
            ServicesModified?.Invoke();
        }

        private async Task Volver()
        {
            RefrescarGrilla();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
