using LoginsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using SQLite;

namespace LoginsAdmin.Repository
{
    public class RepositoryServicios
    {
        SQLiteConnection conn;

        public string StatusMessage { get; set; }

        public RepositoryServicios(string dbPath)
        {
            conn = new SQLiteConnection(dbPath);
            conn.CreateTable<Servicio>();
        }

        public void AgregarServicio(Servicio nuevoServicio)
        {
            int result;
            try
            {
                if(nuevoServicio.Id == -1)
                {
                    result = conn.Insert(nuevoServicio);
                    StatusMessage = "Servicio agregado correctamente.";
                }
                else
                {
                    result = conn.Update(nuevoServicio);
                    StatusMessage = "Servicio actualizado correctamente.";
                }
            }
            catch (Exception ex)
            {
                if(ex.Message == "Constraint")
                    StatusMessage = string.Format("Ya existe un servicio con el nombre {0}", nuevoServicio.Name);
                else
                    StatusMessage = string.Format("No se pudo agregar el servicio {0}. Error: {1}", nuevoServicio.Name, ex.Message);
            }
        }

        public List<Servicio> ObtenerServicios(string textoABuscar = "")
        {
            try
            {
                List<Servicio> listadoServicios = new List<Servicio>();
                if (string.IsNullOrEmpty(textoABuscar))
                {
                    listadoServicios = conn.Table<Servicio>().OrderBy(s => s.Name).ToList();
                }
                else
                {
                   textoABuscar = textoABuscar.ToLowerInvariant();
                    listadoServicios = conn.Table<Servicio>().Where(
                        s => s.Name != "" && (s.Name.ToLower().Contains(textoABuscar)
                                                || s.User.ToLower().Contains(textoABuscar)
                                                || s.Password.ToLower().Contains(textoABuscar)
                                                || s.ExtraData.ToLower().Contains(textoABuscar)
                                            )).OrderBy(s => s.Name).ToList();
                }
                StatusMessage = listadoServicios.Count.ToString();
                return listadoServicios;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("No se pudo obtener la lista de servicios. Error: {0}", ex.Message);
            }

            return new List<Servicio>();
        }
    }
}
