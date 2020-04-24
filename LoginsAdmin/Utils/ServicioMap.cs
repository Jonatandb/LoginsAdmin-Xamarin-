using LoginsAdmin.Domain.Models;

namespace LoginsAdmin.Utils
{
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
