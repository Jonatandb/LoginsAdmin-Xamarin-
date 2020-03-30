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
            conn.CreateTable<Usuario>();
            conn.CreateTable<Servicio>();
        }

        public bool AgregarEditarServicio(Servicio servicio)
        {
            StatusMessage = "";
            int result;
            try
            {
                if (servicio.Id == -1)
                {
                    Servicio temp = conn.Find<Servicio>(s => s.Name.ToLower() == servicio.Name.ToLower());
                    if (temp == null)
                    {

                        result = conn.Insert(servicio);
                        StatusMessage = "Servicio agregado correctamente.";
                    }
                    else
                    {
                        throw new Exception("Constraint");
                    }
                }
                else
                {
                    bool shouldUpdate = false;
                    Servicio temp = conn.Find<Servicio>(s => s.Name.ToLower() == servicio.Name.ToLower());
                    if (temp == null)
                    {
                        // El nuevo nombre es único
                        shouldUpdate = true;
                    }
                    else
                    {
                        // El nuevo nombre pertenece a un servicio existente
                        if(temp.Id == servicio.Id)
                        {
                            // Dejaron el nombre igual
                            shouldUpdate = true;
                        }
                    }
                    if (shouldUpdate)
                    {
                        result = conn.Update(servicio);
                        StatusMessage = "Servicio actualizado correctamente.";
                    }
                    else
                    {
                        throw new Exception("Constraint");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Constraint")
                    StatusMessage = string.Format("Ya existe un servicio con el nombre {0}", servicio.Name);
                else
                    StatusMessage = string.Format("No se pudo agregar el servicio {0}.\n\nDetalles del error: \n{1}", servicio.Name, ex.Message);
                result = 0;
            }
            return result != 0;
        }

        public bool EliminarServicio(int serviceId)
        {
            StatusMessage = "";
            int result;
            try
            {
                result = conn.Delete(new Servicio() { Id = serviceId });
                StatusMessage = "Servicio eliminado correctamente.";
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("No se pudo eliminar el servicio.\n\nDetalles del error: \n{0}", ex.Message);
                result = 0;
            }
            return result != 0;
        }

        public List<Servicio> ObtenerServicios(string textoABuscar = "")
        {
            StatusMessage = "";
            try
            {
                List<Servicio> listadoServicios = new List<Servicio>();
                if (string.IsNullOrEmpty(textoABuscar))
                {
                    listadoServicios = conn.Table<Servicio>().OrderBy(s => s.Name).ToList();
                }
                else
                {
                   textoABuscar = textoABuscar.Trim().ToLowerInvariant();
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
                StatusMessage = string.Format("No se pudo obtener la lista de servicios.\n\nDetalles del error: \n{0}", ex.Message);
            }

            return new List<Servicio>();
        }

        public Usuario ObtenerUsuarioPrincipal()
        {
            StatusMessage = "";
            try
            {
                Usuario usuario = conn.Find<Usuario>(1);
                
                // Si no lo encontré, lo creo con una clave por defecto:
                if(usuario == null)
                {
                    conn.Insert(new Usuario()
                    {
                        UserName = "jonatandb",
                        Password = "loginsadmindefaultpassword"
                    });
                    usuario = conn.Find<Usuario>(1);
                }
                // /*DEBUG*/ usuario.Password = "loginsadmindefaultpassword";                /// ***** Fuerzo a que siempre se pida crear una clave
                return usuario;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("No se pudo acceder a la base de datos.\n\nDetalles del error: \n{0}", ex.Message);
            }
            return null;
        }

        public bool EstablecerClaveUsuarioPrincipal(string password)
        {
            bool result = false;
            StatusMessage = "";
            try
            {
                Usuario usuario = conn.Find<Usuario>(1);
                usuario.Password = password;
                conn.Update(usuario);
                result = true;
            }
            catch (Exception ex) {
                this.StatusMessage = ex.Message;
            }
            return result;
        }

        public bool Login(string password)
        {
            StatusMessage = "";
            bool result = false;
            try
            {
                Usuario usuario = conn.Find<Usuario>(1);
                result = usuario.Password == password;
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
            return result;
        }
    }
}
