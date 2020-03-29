using Xamarin.Forms;
using System.ComponentModel;
using LoginsAdmin.Domain.Models;
using System.Windows.Input;
using System;

namespace LoginsAdmin.Presentation.ViewModels
{
    public class ABMViewModel : INotifyPropertyChanged
    {
        string _nombre, _usuario, _clave, _otrosDatos;

        private Servicio ServiceToEdit { get; set; }

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
                        Id = ServiceToEdit != null ? ServiceToEdit.Id : -1,
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

            EliminarCommand = new Command((idService) =>
            {
                if (App.RepoServicios.EliminarServicio((int)idService))
                {
                    Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                                                App.RepoServicios.StatusMessage,
                                                                "Cerrar");
                    RefrescarGrilla();
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("LoginsAdmin",
                                                                App.RepoServicios.StatusMessage,
                                                                "Cerrar");
                }
            });
        }

        internal void SetServiceToEdit(Servicio service)
        {
            if (service != null)
            {
                ServiceToEdit = service;
                Nombre = ServiceToEdit.Name;
                Usuario = ServiceToEdit.User;
                Clave = ServiceToEdit.Password;
                OtrosDatos = ServiceToEdit.ExtraData;
            }
        }

        private void RefrescarGrilla()
        {
            ServicesModified?.Invoke(null, null);
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                var args = new PropertyChangedEventArgs(nameof(Nombre));
                PropertyChanged?.Invoke(this, args);
            } 
        }

        public string Usuario
        {
            get => string.IsNullOrWhiteSpace(_usuario) ? "" : _usuario;
            set
            {
                _usuario= value;
                var args = new PropertyChangedEventArgs(nameof(Usuario));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public string Clave
        {
            get => string.IsNullOrWhiteSpace(_clave) ? "" : _clave;
            set
            {
                _clave= value;
                var args = new PropertyChangedEventArgs(nameof(Clave));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public string OtrosDatos
        {
            get => string.IsNullOrWhiteSpace(_otrosDatos) ? "" : _otrosDatos;
            set
            {
                _otrosDatos= value;
                var args = new PropertyChangedEventArgs(nameof(OtrosDatos));
                PropertyChanged?.Invoke(this, args);
            }
        }


    }
}
