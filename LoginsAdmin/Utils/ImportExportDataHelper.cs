using LoginsAdmin.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OfficeOpenXml;

namespace LoginsAdmin.Utils
{
    public class ImportExportDataHelper
    {
        public static string StatusMessage { get; internal set; }

        private static string GetAppPassword()
        {
            return App.RepoServicios.ObtenerUsuarioPrincipal().Password;
        }

        public static string GetBackupFilePath()
        {
            return Path.Combine(App.OutputFilesFolderPath, App.BackupDataFileName);
        }

        private static string GetCurrentDateTimeString()
        {
            DateTime now = DateTime.Now;
            string dateTimeString = now.ToString("dd-MM-yyyy__HH-mm'hs'");
            return dateTimeString;
        }

        private static void GenerateHeaders(ExcelWorksheet worksheet)
        {
            worksheet.Cells["A1"].Value = "Id";
            worksheet.Cells["B1"].Value = "Servicio";
            worksheet.Cells["C1"].Value = "Usuario";
            worksheet.Cells["D1"].Value = "Clave";
            worksheet.Cells["E1"].Value = "Otros datos";
        }

        internal static bool ExportData()
        {
            bool result = false;

            try
            {
                if (App.RepoServicios.ObtenerServicios().Count == 0)
                {
                    StatusMessage = "No hay datos para exportar.";
                    return false;
                }

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                
                string exportDataFilePath = GetBackupFilePath();

                // Crear el archivo de Excel
                using (ExcelPackage package = new ExcelPackage())
                {
                    // Agregar una nueva hoja de cálculo al libro
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(GetCurrentDateTimeString());

                    // Agregar contenido a la hoja de cálculo
                    GenerateHeaders(worksheet);

                    int row = 3;
                    foreach (Servicio service in App.RepoServicios.ObtenerServicios())
                    {
                        worksheet.Cells["A" + row.ToString()].Value = service.Id;
                        worksheet.Cells["B" + row.ToString()].Value = service.Name;
                        worksheet.Cells["C" + row.ToString()].Value = service.User;
                        worksheet.Cells["D" + row.ToString()].AddComment(service.Password);
                        worksheet.Cells["E" + row.ToString()].Value = service.ExtraData;
                        row++;
                    }

                    // Proteger el archivo de Excel con contraseña
                    package.Encryption.Password = GetAppPassword();

                    // Guardar el archivo de Excel
                    package.SaveAs(new FileInfo(exportDataFilePath));
                }

                StatusMessage = exportDataFilePath;
                result = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Log(ex.Message, "ImportExportDataHelper.ExportData()");
                if (ex.Message.Contains("saving"))
                {
                    StatusMessage = "No se pudo crear el archivo: " + GetBackupFilePath() + "\n\nVerifique que la aplicación tiene permiso para acceder al almacenamiento interno: 'App permissions -> Storage'.";
                }
                else
                {
                    StatusMessage = ex.Message;
                }
            }
            return result;
        }

        internal static bool ImportData()
        {
            bool result = false;

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                string filePath = GetBackupFilePath();
                string password = GetAppPassword();

                using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath), password))
                {

                    ExcelWorksheet worksheet;

                    try
                    {
                        // Acceso a la primera hoja de trabajo
                        worksheet = package.Workbook.Worksheets[0];
                    }
                    catch
                    {
                        throw new Exception("No se pudo abrir el archivo: " + filePath + "\n\nVerifique que el archivo existe y que la aplicación tiene permiso para acceder a él: 'App permissions -> Storage'.");
                    }

                    var importedServices = new List<Servicio>();
                    int row = 3;
                    try
                    {
                        while (worksheet.Cells["B" + row.ToString()].Value != null)
                        {
                            var newService = new Servicio();
                            newService.Id = -1;

                            newService.Name = "";
                            newService.User = "";
                            newService.Password = "";
                            newService.ExtraData = "";
                            try
                            {
                                newService.Name = worksheet.Cells["B" + row.ToString()].Value.ToString();
                            } catch{}
                            try
                            {
                                newService.User = worksheet.Cells["C" + row.ToString()].Value.ToString();
                            } catch{}
                            try
                            {
                                newService.Password = worksheet.Cells["D" + row.ToString()].Comment.Text;
                            } catch{}
                            try
                            {
                                newService.ExtraData = worksheet.Cells["E" + row.ToString()].Value.ToString();
                            } catch {}
                            importedServices.Add(newService);
                            row++;
                        }
                    }
                    catch { }
                    finally
                    {
                        package.Dispose();
                    }

                    var insertados = 0;
                    var erroneos = new List<string>();
                    if (importedServices.Count > 0)
                    {
                        foreach(Servicio service in importedServices)
                        {
                            if (App.RepoServicios.AgregarEditarServicio(service))
                            {
                                insertados++;
                            }
                            else
                            {
                                erroneos.Add(App.RepoServicios.StatusMessage);
                            }
                        }
                        StringBuilder importResults = new StringBuilder();
                        if (insertados > 0)
                            importResults.AppendLine("Importados: " + insertados);
                        if (erroneos.Count > 0)
                        {
                            importResults.AppendLine("Erróneos: " + erroneos.Count);
                            importResults.AppendLine("Detalle de los errores:");
                            erroneos.ForEach(e => importResults.AppendLine(e));
                        }
                        StatusMessage = importResults.ToString();
                    }
                    else
                    {
                        StatusMessage = "No se encontraron datos para importar.";
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Log(ex.Message, "ImportExportDataHelper.ImportData()");
                StatusMessage = ex.Message + "\n\nVerifique que el archivo existe y que la aplicación tiene permiso para acceder a él: 'App permissions -> Storage'.";
            }
            return result;
        }
    }
}
