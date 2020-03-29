using Xamarin.Forms;
using System.ComponentModel;
using LoginsAdmin.Domain.Models;
using System.Windows.Input;
using System;

namespace LoginsAdmin.Presentation.ViewModels
{
    public class ABMViewModel : INotifyPropertyChanged
    {
        int _id;
        string _nombre, _usuario, _clave, _otrosDatos;

        private Servicio ServiceToEdit { get; set; }
        public bool IsEditMode { get => ServiceToEdit != null; }
        public bool IsValidServiceName { get => !string.IsNullOrWhiteSpace(Nombre); }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler ServicesModified;

        public ICommand GuardarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }

        public ABMViewModel()
        {
            GuardarCommand = new Command(() =>
            {
                if (string.IsNullOrWhiteSpace(Nombre))
                {
                    App.Current.MainPage.DisplayAlert("Atención",
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
                        Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                                                  App.RepoServicios.StatusMessage,
                                                                  "Cerrar");
                        RefrescarGrilla();

                        Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                                                  App.RepoServicios.StatusMessage,
                                                                  "Cerrar");
                    }
                }
            });

            EliminarCommand = new Command(() =>
            {
                if(IsEditMode)
                {
                    if (App.RepoServicios.EliminarServicio(ServiceToEdit.Id))
                    {
                        Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                                                    App.RepoServicios.StatusMessage,
                                                                    "Cerrar");
                        RefrescarGrilla();

                        Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                                                    App.RepoServicios.StatusMessage,
                                                                    "Cerrar");
                    }
                }
            });
        }

        internal void SetServiceToEdit(Servicio service)
        {
            if (service != null)
            {
                ServiceToEdit = service;
                Id = ServiceToEdit.Id;
                Nombre = ServiceToEdit.Name;
                Usuario = ServiceToEdit.User;
                Clave = ServiceToEdit.Password;
                OtrosDatos = ServiceToEdit.ExtraData;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEditMode)));
            }
        }

        private void RefrescarGrilla()
        {
            ServicesModified?.Invoke(null, null);
        }

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
                _nombre = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nombre)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValidServiceName)));
            } 
        }

        public string Usuario
        {
            get => string.IsNullOrWhiteSpace(_usuario) ? "" : _usuario;
            set
            {
                _usuario= value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Usuario)));
            }
        }

        public string Clave
        {
            get => string.IsNullOrWhiteSpace(_clave) ? "" : _clave;
            set
            {
                _clave= value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Clave)));
            }
        }

        public string OtrosDatos
        {
            get => string.IsNullOrWhiteSpace(_otrosDatos) ? "" : _otrosDatos;
            set
            {
                _otrosDatos= value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OtrosDatos)));
            }
        }


    }
}
