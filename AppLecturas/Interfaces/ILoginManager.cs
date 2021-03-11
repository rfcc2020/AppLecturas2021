using System;
using System.Collections.Generic;
using System.Text;

namespace AppLecturas.Interfaces
{
    //interfaz para manejar el control de usuario
    public interface ILoginManager
    {
        void ShowMainPage();//método que mostrará la página principal
        void Logout();//metodo que cerrará la sesión
    }
}
