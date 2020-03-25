using LoginsAdmin.Presentation;
using LoginsAdmin.Repository;
using LoginsAdmin.Utils;
using Xamarin.Forms;
namespace LoginsAdmin
{
    public partial class App : Application
    {
        string dbPath => FileAccessHelper.GetLocalFilePath("loginsadmin.db3");

        public static RepositoryServicios RepoServicios { get; private set; }

        public App()
        {
            InitializeComponent();

            RepoServicios = new RepositoryServicios(dbPath);

            MainPage = new Inicio();
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
