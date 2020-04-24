using LoginsAdmin.Presentation;
using LoginsAdmin.Repository;
using LoginsAdmin.Utils;
using System;
using Xamarin.Forms;
namespace LoginsAdmin
{
    public partial class App : Application
    {
        public static string DataBaseName = "loginsadmin.db3";
        public static string ErrorsFileName = "LoginsAdmin_Errores.txt";
        public static string BackupDataFileName = "LoginsAdmin_Datos.csv";

        public static string OutputFilesFolderPath { get; set; }

        public static bool IsUserLoggedIn { get; set; }

        public static RepositoryServicios RepoServicios { get; private set; }

        public App(string externalStorageDownloadsDirectoryPath)
        {
            InitializeComponent();

            OutputFilesFolderPath = externalStorageDownloadsDirectoryPath;

            try
            {
                RepoServicios = new RepositoryServicios();

                if (!IsUserLoggedIn)
                {
                    ContentPage loginPage = new Login();
                    NavigationPage.SetHasBackButton(loginPage, false);
                    NavigationPage.SetHasNavigationBar(loginPage, false);
                    MainPage = new NavigationPage(loginPage);
                }
                else
                {
                    MainPage = new Inicio();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log(ex.Message, "App.App()");
                MainPage = new ErrorSQLite();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
