using LoginsAdmin.Domain.Models;
using LoginsAdmin.Repository;
using System;
using System.Collections.Generic;
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
        private int idServicioSeleccionado;

        public Inicio()
        {
            InitializeComponent();
            idServicioSeleccionado = -1;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RecargarGrilla();
        }

        private async void RecargarGrilla(string textoABuscar = "")
        {
            List<Servicio> servicios = App.RepoServicios.ObtenerServicios(textoABuscar);
            lstServicios.ItemsSource = servicios;
            if (Int32.TryParse(App.RepoServicios.StatusMessage, out int cantidadRegistros))
            {
                string resultadosBusqueda = "Aún no se agregaron servicios...";
                if (cantidadRegistros == 0)
                {
                    if (!string.IsNullOrEmpty(txtBusqueda.Text))
                    {
                        resultadosBusqueda = "No hay servicios que conincidan con la búqueda actual...";
                    }
                }
                else
                {
                    resultadosBusqueda = string.Format("Servicios: {0}", cantidadRegistros);
                }
                lblEstado.Text = resultadosBusqueda;
            }
            else
            {
                await this.DisplayAlert(
                                    "Atención",
                                    App.RepoServicios.StatusMessage,
                                    "Cerrar");
            }
        }

        private async void btnAgregarServicio_Clicked(object sender, EventArgs e)
        {
            //basic validation to ensure a name was entered
            if (string.IsNullOrEmpty(txtNombreServicio.Text) || string.IsNullOrEmpty(txtNombreServicio.Text.Trim()))
            {
                await this.DisplayAlert(
                                    "Atención",
                                    "Se requiere que al menos se especifíque el nombre del nuevo servicio.",
                                    "Cerrar");
                txtNombreServicio.Focus();
            }
            else
            {
                Servicio nuevoServicio = new Servicio();

                nuevoServicio.Id = idServicioSeleccionado;
                nuevoServicio.Name = txtNombreServicio.Text;
                nuevoServicio.User = txtUsuarioServicio.Text;
                nuevoServicio.Password = txtClaveServicio.Text;
                nuevoServicio.ExtraData = txtExtraDataServicio.Text;

                App.RepoServicios.AgregarServicio(nuevoServicio);
                
                txtNombreServicio.Text = "";
                txtUsuarioServicio.Text = "";
                txtClaveServicio.Text = "";
                txtExtraDataServicio.Text = "";

                await this.DisplayAlert(
                                    "LoginsAdmin",
                                    App.RepoServicios.StatusMessage,
                                    "Cerrar");
                idServicioSeleccionado = -1;
                this.btnAgregarServicio.Text = "Agregar";
                txtBusqueda.Text = "";
                RecargarGrilla();
            }
        }

        private void txtBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            RecargarGrilla(e.NewTextValue);
        }

        private void lstServicios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            txtBusqueda.Text = "";
            Servicio servicioSeleccionado = (Servicio) e.SelectedItem;
            this.idServicioSeleccionado = servicioSeleccionado != null ? servicioSeleccionado.Id : -1;
            this.txtNombreServicio.Text = servicioSeleccionado != null ? servicioSeleccionado.Name : "";
            this.txtUsuarioServicio.Text = servicioSeleccionado != null ? servicioSeleccionado.User : "";
            this.txtClaveServicio.Text = servicioSeleccionado != null ? servicioSeleccionado.Password : "";
            this.txtExtraDataServicio.Text = servicioSeleccionado != null ? servicioSeleccionado.ExtraData : "";
            this.btnAgregarServicio.Text = servicioSeleccionado != null ? "Actualizar datos" : "Agregar";
        }

        private void txtBusqueda_Focused(object sender, FocusEventArgs e)
        {
            lstServicios.SelectedItem = null;
        }
    }
}


