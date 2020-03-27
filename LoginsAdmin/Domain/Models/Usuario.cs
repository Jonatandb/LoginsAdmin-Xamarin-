using SQLite;

namespace LoginsAdmin.Domain.Models
{
    [Table("users")]
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250), Unique]
        public string UserName { get; set; }

        [MaxLength(250)]
        public string Password { get; set; }
    }
}
