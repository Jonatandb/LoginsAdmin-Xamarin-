using LoginsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using SQLite;
using System.Threading.Tasks;

namespace LoginsAdmin.Repository
{
    public class RepositoryServicios
    {
        //SQLiteAsyncConnection conn;
        SQLiteConnection conn;

        public string StatusMessage { get; set; }

        public RepositoryServicios(string dbPath)
        {
            //conn = new SQLiteAsyncConnection(dbPath);
            conn = new SQLiteConnection(dbPath);
            //conn.CreateTableAsync<Usuario>();
            conn.CreateTable<Usuario>();
            //conn.CreateTableAsync<Servicio>();
            conn.CreateTable<Servicio>();
        }

        //public async void AgregarServicio(Servicio nuevoServicio)
        public void AgregarServicio(Servicio nuevoServicio)
        {
            int result;
            StatusMessage = "";
            try
            {
                if(nuevoServicio.Id == -1)
                {
                    //result = await conn.InsertAsync(nuevoServicio);
                    result = conn.Insert(nuevoServicio);
                    StatusMessage = "Servicio agregado correctamente.";
                }
                else
                {
                    //result = await conn.UpdateAsync(nuevoServicio);
                    result = conn.Update(nuevoServicio);
                    StatusMessage = "Servicio actualizado correctamente.";
                }
            }
            catch (Exception ex)
            {
                if(ex.Message == "Constraint")
                    StatusMessage = string.Format("Ya existe un servicio con el nombre {0}", nuevoServicio.Name);
                else
                    StatusMessage = string.Format("No se pudo agregar el servicio {0}.\n\nDetalles del error: \n{1}", nuevoServicio.Name, ex.Message);
            }
        }

        //public async Task<List<Servicio>> ObtenerServicios(string textoABuscar = "")
        public List<Servicio> ObtenerServicios(string textoABuscar = "")
        {
            StatusMessage = "";
            try
            {
                List<Servicio> listadoServicios = new List<Servicio>();
                if (string.IsNullOrEmpty(textoABuscar))
                {
          //          listadoServicios = await conn.Table<Servicio>().OrderBy(s => s.Name).ToListAsync();
                    listadoServicios = conn.Table<Servicio>().OrderBy(s => s.Name).ToList();
                }
                else
                {
                   textoABuscar = textoABuscar.ToLowerInvariant();
                    //listadoServicios = await conn.Table<Servicio>().Where(
                    listadoServicios = conn.Table<Servicio>().Where(
                        s => s.Name != "" && (s.Name.ToLower().Contains(textoABuscar)
                                                || s.User.ToLower().Contains(textoABuscar)
                                                || s.Password.ToLower().Contains(textoABuscar)
                                                || s.ExtraData.ToLower().Contains(textoABuscar)
                                            //)).OrderBy(s => s.Name).ToListAsync();
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

        //public async Task<Usuario> ObtenerUsuarioPrincipal()
        public Usuario ObtenerUsuarioPrincipal()
        {
            StatusMessage = "";
            try
            {
                //Usuario usuario = await conn.FindAsync<Usuario>(1);
                Usuario usuario = conn.Find<Usuario>(1);
                
                // Si no lo encontré, lo creo con una clave por defecto:
                if(usuario == null)
                {
                    //await conn.InsertAsync(new Usuario()
                    conn.Insert(new Usuario()
                    {
                        UserName = "jonatandb",
                        Password = "loginsadmindefaultpassword"
                    });
                    //usuario = await conn.FindAsync<Usuario>(1);
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

        //public async Task<bool> EstablecerClaveUsuarioPrincipal(string password)
        public bool EstablecerClaveUsuarioPrincipal(string password)
        {
            bool result = false;
            StatusMessage = "";
            try
            {
                //Usuario usuario = await conn.FindAsync<Usuario>(1);
                Usuario usuario = conn.Find<Usuario>(1);
                usuario.Password = password;
                //await conn.UpdateAsync(usuario);
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
            bool result = false;
            StatusMessage = "";
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
