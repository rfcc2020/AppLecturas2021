using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppLecturas.Modelo
{
    //clase que modela la tabla persona
    public class ClsPersona:ClsBase
    {
        [PrimaryKey]
        public int Id { get; set; }
       // [Unique]
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
    }
}
