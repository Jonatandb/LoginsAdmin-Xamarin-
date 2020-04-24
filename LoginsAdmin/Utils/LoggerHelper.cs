using System;
using System.IO;

namespace LoginsAdmin.Utils
{
    public class LoggerHelper
    {
        internal static void Log(string what, string where = "")
        {
            try
            {
                var errorsFileFullPath = Path.Combine(App.OutputFilesFolderPath, App.ErrorsFileName);
                using (StreamWriter sw = new StreamWriter(errorsFileFullPath))
                {
                    var when = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    var separator1 = ": ";
                    var separator2 = " -> ";
                    sw.Write(when + separator1 + (where != "" ? where + separator2 : "") + what);
                }
            }
            catch
            {
                // It can't log anything...
            }
        }
    }
}
