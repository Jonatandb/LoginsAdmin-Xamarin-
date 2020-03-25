using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace LoginsAdmin.Domain.Models
{
    [Table("services")]
    public class Servicio
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250), Unique]
        public string Name { get; set; }
        
        [MaxLength(250)]
        public string User { get; set; }
        
        [MaxLength(250)]
        public string Password { get; set; }

        [MaxLength(1000)]
        public string ExtraData { get; set; }
    }
}
