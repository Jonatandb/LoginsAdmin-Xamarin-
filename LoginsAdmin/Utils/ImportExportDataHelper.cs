using CsvHelper;
using LoginsAdmin.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace LoginsAdmin.Utils
{
    public class ImportExportDataHelper
    {
        public static string StatusMessage { get; internal set; }

        internal static bool ExportData()
        {
            bool result = false;
            try
            {
                string exportDataFilePath = Path.Combine(App.OutputFilesFolderPath, App.BackupDataFileName);
                using (var writer = new StreamWriter(exportDataFilePath))
                {
                    var csvConfig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture);
                    csvConfig.Encoding = Encoding.UTF8;
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.Configuration.RegisterClassMap<ServicioMap>();
                        csv.WriteRecords(ObtenerRegistrosSanitizados());
                    }
                }
                StatusMessage = exportDataFilePath;
                result = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Log(ex.Message, "ImportExportDataHelper.ExportData()");
                StatusMessage = ex.Message;
            }
            return result;
        }

        private static IEnumerable ObtenerRegistrosSanitizados()
        {
            var servicios = App.RepoServicios.ObtenerServicios();
            try
            {
                servicios.ForEach(servicio => servicio.ExtraData = servicio.ExtraData.Replace('\n', ' ')); // Reemplazo saltos de línea por espacios
            }
            catch (Exception ex)
            {
                LoggerHelper.Log(ex.Message, "ImportExportDataHelper.ObtenerRegistrosSanitizados()");
            }
            return servicios;
        }

        internal static bool ImportData()
        {
            bool result = false;
            var insertados = 0;
            var erroneos = new List<string>();
            try
            {
                using (var reader = new StreamReader(Path.Combine(App.OutputFilesFolderPath, App.BackupDataFileName)))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.RegisterClassMap<ServicioMap>();
                    var records = csv.GetRecords<Servicio>().ToList();
                    records.ForEach(servicio =>
                    {
                        servicio.Id = -1;
                        if (App.RepoServicios.AgregarEditarServicio(servicio))
                        {
                            insertados++;
                        }
                        else
                        {
                            erroneos.Add(App.RepoServicios.StatusMessage);
                        }
                    });
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
                result = true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Log(ex.Message, "ImportExportDataHelper.ImportData()");
                StatusMessage = ex.Message;
            }
            return result;
        }
    }
}
