using Xamarin.Forms;
using System.ComponentModel;
using LoginsAdmin.Domain.Models;
using System.Windows.Input;
using System.Threading.Tasks;
using System;
using LoginsAdmin.Utils;

namespace LoginsAdmin.Presentation.ViewModels
{
    public class ABMViewModel : INotifyPropertyChanged
    {
        string _nombre, _usuario, _clave, _otrosDatos;
        
        public delegate void ServicesModifiedEventHandler();
        
        public event ServicesModifiedEventHandler ServicesModified;
        
        public event PropertyChangedEventHandler PropertyChanged;


        public ABMViewModel()
        {
            GuardarCommand = new Command(Guardar, IsSaveButtonEnabled);
            EliminarCommand = new Command(Eliminar);
        }



        public ICommand GuardarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }

        private Servicio ServiceToEdit { get; set; }
        
        public bool IsEditMode { get => ServiceToEdit != null; }
        
        private int Id { get; set; }

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (_nombre == value) return;
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
                ((Command)GuardarCommand).ChangeCanExecute();
            }
        }

        public string Usuario
        {
            get => _usuario;
            set
            {
                if (_usuario == value) return;
                _usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        public string Clave
        {
            get => _clave;
            set
            {
                if (_clave == value) return;
                _clave = value;
                OnPropertyChanged(nameof(Clave));
            }
        }

        public string OtrosDatos
        {
            get => _otrosDatos;
            set
            {
                if (_otrosDatos == value) return;
                _otrosDatos = value;
                OnPropertyChanged(nameof(OtrosDatos));
            }
        }


        private bool IsSaveButtonEnabled()
        {
            return !string.IsNullOrWhiteSpace(Nombre);
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

        private async void Guardar()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Nombre))
                {
                    await App.Current.MainPage.DisplayAlert(
                        "Atención",
                        "Se requiere que al menos se especifíque un nombre para el servicio.",
                        "Aceptar");
                }
                else
                {
                    Servicio servicio = new Servicio
                    {
                        Id = IsEditMode ? ServiceToEdit.Id : -1,
                        Name = Nombre.Trim(),
                        User = string.IsNullOrWhiteSpace(Usuario) ? "" : Usuario.Trim(),
                        Password = string.IsNullOrWhiteSpace(Clave) ? "" : Clave.Trim(),
                        ExtraData = string.IsNullOrWhiteSpace(OtrosDatos) ? "" : OtrosDatos.Trim()
                    };

                    bool success = App.RepoServicios.AgregarEditarServicio(servicio);
                    await Application.Current.MainPage.DisplayAlert(
                        "LoginsAdmin",
                        App.RepoServicios.StatusMessage,
                        "Cerrar");
                    if (success)
                        await Volver();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log(ex.Message, "ABMViewModel.Guardar()");
                await App.Current.MainPage.DisplayAlert(
                    "Atención",
                    "No se pudo guardar el usuario.\n\nPor favor contactarse con jonatandb@gmail.com\n\nABMViewModel.Guardar(): " + ex.Message,
                    "Aceptar");
            }
        }

        private async void Eliminar()
        {
            try
            {
                bool respuesta = await Application.Current.MainPage.DisplayAlert(
                    "Atención", 
                    "¿Está seguro que desea eliminar este servicio?", 
                    "Si", 
                    "No");
                if (respuesta)
                {
                    if (IsEditMode)
                    {
                        if (App.RepoServicios.EliminarServicio(ServiceToEdit.Id))
                        {
                            await Volver();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(
                                "LoginsAdmin",
                                App.RepoServicios.StatusMessage,
                                "Cerrar");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log(ex.Message, "ABMViewModel.Eliminar()");
                await App.Current.MainPage.DisplayAlert(
                    "Atención",
                   "No se pudo eliminar el usuario.\n\nPor favor contactarse con jonatandb@gmail.com\n\nABMViewModel.Eliminar(): " + ex.Message,
                    "Aceptar");
            }
        }

        private async Task Volver()
        {
            RefrescarGrilla();
            await Application.Current.MainPage.Navigation.PopAsync(false);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
