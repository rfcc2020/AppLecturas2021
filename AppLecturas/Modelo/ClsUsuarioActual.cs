using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppLecturas.Modelo
{
    //clase que modela la tabla usuario actual
    public class ClsUsuarioActual:ClsBase
    {
        [PrimaryKey]
        public int Id { get; set; }//propiedad
        public string Name { get; set; }
        [Unique]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Sector { get; set; }        
    }
}
