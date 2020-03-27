using LoginsAdmin.Presentation;
using LoginsAdmin.Repository;
using LoginsAdmin.Utils;
using Xamarin.Forms;
namespace LoginsAdmin
{
    public partial class App : Application
    {
        string dbPath => FileAccessHelper.GetLocalFilePath("loginsadmin.db3");

        public static bool IsUserLoggedIn { get; set; }

        public static RepositoryServicios RepoServicios { get; private set; }

        public App()
        {
            InitializeComponent();
            try
            {
                RepoServicios = new RepositoryServicios(dbPath);
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
            catch (System.Exception)
            {
                // Se produjo algún error relacionado con las tablas...
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
