using LoginsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using LoginsAdmin.Utils;
using System.IO;

namespace LoginsAdmin.Repository
{
    public class RepositoryServicios
    {
        SQLiteConnection conn;


        public RepositoryServicios()
        {
            conn = new SQLiteConnection(FileAccessHelper.GetLocalDatabaseFilePath());
            conn.CreateTable<Usuario>();
            conn.CreateTable<Servicio>();
            VerifySchemaAgainstReleaseV1_alpha();
        }


        public string StatusMessage { get; set; }


        public bool AgregarEditarServicio(Servicio servicio)
        {
            StatusMessage = "";
            int result = 0;
            try
            {
                if (servicio.Id == -1)
                {
                    result = conn.Insert(servicio);
                    StatusMessage = "Servicio agregado correctamente.";
                }
                else
                {
                    result = conn.Update(servicio);
                    StatusMessage = "Servicio actualizado correctamente.";
                }
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("No se pudo agregar el servicio {0}.\n\nDetalles del error: \n{1}", servicio.Name, ex.Message);
                LoggerHelper.Log(errorMessage, "RepositoryServicios.AgregarEditarServicio()");
                StatusMessage = errorMessage;
            }
            return result != 0;
        }

        public bool EliminarServicio(int serviceId)
        {
            StatusMessage = "";
            int result = 0;
            try
            {
                result = conn.Delete(new Servicio() { Id = serviceId });
                StatusMessage = "Servicio eliminado correctamente.";
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("No se pudo eliminar el servicio.\n\nDetalles del error: \n{0}", ex.Message);
                LoggerHelper.Log(errorMessage, "RepositoryServicios.EliminarServicio()");
                StatusMessage = errorMessage;
            }
            return result != 0;
        }

        public List<Servicio> ObtenerServicios(string textoABuscar = "")
        {
            StatusMessage = "";
            List<Servicio> listadoServicios = new List<Servicio>();
            try
            {
                if (string.IsNullOrEmpty(textoABuscar))
                {
                    listadoServicios = conn.Table<Servicio>().OrderBy(s => s.Name, StringComparer.OrdinalIgnoreCase).ToList();
                }
                else
                {
                   textoABuscar = textoABuscar.Trim().ToLowerInvariant();
                    listadoServicios = conn.Table<Servicio>().Where(
                        s => s.Name != "" && (s.Name.ToLower().Contains(textoABuscar)
                                                || s.User.ToLower().Contains(textoABuscar)
                                                || s.Password.ToLower().Contains(textoABuscar)
                                                || s.ExtraData.ToLower().Contains(textoABuscar)
                                            )).OrderBy(s => s.Name, StringComparer.OrdinalIgnoreCase).ToList();
                }
                StatusMessage = listadoServicios.Count.ToString();
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("No se pudo obtener la lista de servicios.\n\nDetalles del error: \n{0}", ex.Message);
                LoggerHelper.Log(errorMessage, "RepositoryServicios.ObtenerServicios()");
                StatusMessage = errorMessage;
            }
            return listadoServicios;
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
                // /*DEBUG*/ usuario.Password = "loginsadmindefaultpassword";
                return usuario;
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("No se pudo acceder a la base de datos.\n\nDetalles del error: \n{0}", ex.Message);
                LoggerHelper.Log( errorMessage, "RepositoryServicios.ObtenerUsuarioPrincipal()");
                StatusMessage = errorMessage;
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
            catch (Exception ex)
            {
                LoggerHelper.Log(ex.Message, "RepositoryServicios.EstablecerClaveUsuarioPrincipal()");
                StatusMessage = ex.Message;
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
                LoggerHelper.Log( ex.Message, "RepositoryServicios.Login()");
                StatusMessage = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Si la tabla "services" prohíbe insertar registros con "Name" duplicado,
        /// es una base de una versión anterior entonces llevo a cabo la migración.
        /// </summary>
        public void VerifySchemaAgainstReleaseV1_alpha()
        {
            if (!CanInsertDuplicates())
            {
                MigrateSchemaToV1();
            }
        }

        private bool CanInsertDuplicates()
        {
            bool result = true;
            var testService = new Servicio() { Name = "LoginsAdmin_v1.0-alpha_schema_validation" };
            try
            {
                // Intento insertar registros duplicados:
                conn.Insert(testService);
                conn.Insert(testService);

                // Elimino registros de verificación ya que no es necesario migrar:
                conn.Execute("delete from services where name = ?", testService.Name);
                return result;
            }
            catch (Exception ex)
            {
                conn.Execute("delete from services where name = ?", testService.Name);
                if (ex.Message == "Constraint")
                {
                    // Se produjo un error por Name duplicado.
                    result = false;
                }
                else
                {
                    LoggerHelper.Log(ex.Message, "RepositoryServicios.CanInsertDuplicates()");
                    throw ex;
                }
            }
            return result;
        }

        private void MigrateSchemaToV1()
        {
            try
            {
                conn.Execute("ALTER TABLE 'services' RENAME TO 'services_old'");
                conn.CreateTable<Servicio>();
                conn.Execute("INSERT INTO services (Name, User, Password, ExtraData) SELECT Name, User, Password, ExtraData FROM 'services_old'");
                conn.Execute("DROP TABLE services_old;");
            }
            catch (Exception migrationException)
            {
                LoggerHelper.Log( migrationException.Message, "RepositoryServicios.MigrateSchemaToV1()");
                throw migrationException;
            }
        }

        private static void _debug_CopyDatabaseToOutputFilesFolder()
        {
            try
            {
                var currentDBPath = FileAccessHelper.GetLocalFilePath(App.DataBaseName);
                var destinationDBPath = Path.Combine(App.OutputFilesFolderPath, App.DataBaseName);
                if (File.Exists(destinationDBPath))
                    File.Delete(destinationDBPath);
                File.Copy(currentDBPath, destinationDBPath);
                LoggerHelper.Log("Database copied -> " + destinationDBPath, "_debug_CopyDatabaseToOutputFilesFolder()");
            }
            catch (Exception ex)
            {
                LoggerHelper.Log(ex.Message, "_debug_CopyDatabaseToOutputFilesFolder()");
            }
        }
    }
}
