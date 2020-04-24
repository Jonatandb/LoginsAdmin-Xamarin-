using System.IO;
using Xamarin.Essentials;

namespace LoginsAdmin.Utils
{
    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string filename)
        {
            return Path.Combine(FileSystem.AppDataDirectory, filename);
        }

        public static string GetLocalDatabaseFilePath()
        {
            return GetLocalFilePath(App.DataBaseName);
        }
    }
}