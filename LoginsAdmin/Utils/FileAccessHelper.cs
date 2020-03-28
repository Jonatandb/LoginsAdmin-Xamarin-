using System;
using System.IO;
using Xamarin.Essentials;

namespace LoginsAdmin.Utils
{
    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string filename)
        {
            return System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);
        }

        internal static async System.Threading.Tasks.Task ExportDataAsync(string downloadsPath)
        {
            try
            {
                var backingFile = Path.Combine(downloadsPath, "loginsadmin.servicios.txt");
                using (var writer = File.CreateText(backingFile))
                {
                    await writer.WriteLineAsync("Jonatandb test " + new DateTime().ToShortDateString().ToString());
                }
            }
            catch (Exception ex)
            {
                var ups = "Algo no le gustó... "+ ex.Message;
            }
        }
    }
}