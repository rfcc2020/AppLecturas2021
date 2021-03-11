using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppLecturas.Modelo
{
    //clase base de las que heredarán las demás clases del espacio de nombres modelo
    public abstract class ClsBase
    {
        public DateTime Created_at { get; set; }//propiedad
        public DateTime Updated_at { get; set; }//propiedad
    }
}
