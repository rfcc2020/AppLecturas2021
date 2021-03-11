using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppLecturas.Modelo
{
    //clase que modela la tabla persona
    public class ClsPolitica:ClsBase
    {
        [PrimaryKey]
        public int Id { get; set; }
        [Unique]
        public float cantidadConsumo { get; set; }
        public float valorConsumo { get; set; }
        public float valorExeso { get; set; }
       
    }
}
