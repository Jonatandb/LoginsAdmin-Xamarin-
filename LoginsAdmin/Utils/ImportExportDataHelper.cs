using CsvHelper;
using LoginsAdmin.Domain.Models;
using System;
using System.Collections;
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
            string ruta_al_csv = App.BackupFilePath;
            bool result = false;
            try
            {
                using (var writer = new StreamWriter(ruta_al_csv))
                {
                    var csvConfig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture);
                    csvConfig.Encoding = Encoding.UTF8;
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.Configuration.RegisterClassMap<ServicioMap>();
                        csv.WriteRecords(ObtenerRegistrosSanitizados());
                    }
                }
                StatusMessage = ruta_al_csv;
                result = true;
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
            return result;
        }

        private static IEnumerable ObtenerRegistrosSanitizados()
        {
            var servicios = App.RepoServicios.ObtenerServicios();
            servicios.ForEach(servicio => servicio.ExtraData = servicio.ExtraData.Replace('\n', ' ')); // Reemplazo saltos de línea por espacios
            return servicios;
        }

        internal static bool ImportData()
        {
            bool result = false;
            var duplicados = 0;
            var insertados = 0;
            var erroneos = 0;
            try
            {
                string ruta_al_csv = App.BackupFilePath;
                using (var reader = new StreamReader(ruta_al_csv))
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
                            if(App.RepoServicios.StatusMessage == string.Format("Ya existe un servicio con el nombre {0}", servicio.Name))
                            {
                                duplicados++;
                            }
                            else
                            {
                                erroneos++;
                            }
                        }
                    });
                }
                StringBuilder importResults = new StringBuilder();
                if (insertados > 0)
                    importResults.AppendLine("Importados: " + insertados);
                if (duplicados > 0)
                    importResults.AppendLine("Duplicados ignorados: " + duplicados);
                if (erroneos > 0)
                    importResults.AppendLine("Erróneos: " + erroneos);
                StatusMessage = importResults.ToString();
                result = true;
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
            return result;
        }
    }
    
    public class ServicioMap : CsvHelper.Configuration.ClassMap<Servicio>
    {
        public ServicioMap()
        {
            Map(m => m.Id).Index(0).Name("Id");
            Map(m => m.Name).Index(1).Name("Servicio");
            Map(m => m.User).Index(2).Name("Usuario");
            Map(m => m.Password).Index(3).Name("Clave");
            Map(m => m.ExtraData).Index(4).Name("Otros datos");
        }
    }
}
