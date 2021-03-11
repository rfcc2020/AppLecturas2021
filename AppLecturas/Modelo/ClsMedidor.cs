using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppLecturas.Modelo
{
    //clase que modela la tabla medidor
    public class ClsMedidor:ClsBase
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Numero { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; } 
        public string Sector { get; set; }
        public int Persona_id { get; set; }

        public override string ToString()
        {
            return Numero.ToString();
        }
    }
}
