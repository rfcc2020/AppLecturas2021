using AppLecturas.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppLecturas.Vista
{
    //clase para manejar la vista login como una vista modal
    public class LoginModalPage:CarouselPage
    {
        private readonly ContentPage Login;//variable local tipo ContenPage(página de contenido) solo lectura
        public LoginModalPage(ILoginManager ObjIlm)//constructor
        {
            Login = new PagLogin(ObjIlm);//instanciación de un objeto de la clase PagLogin del espacio de nombres vista
            this.Children.Add(Login);//se añade a la pila de vistas de la aplicación la vista login
            MessagingCenter.Subscribe<ContentPage>(this, "Login", (sender) =>//es para seleccionar el formulario login
              {
                  this.SelectedItem = Login;//para mostrar la vista login
              });
        }
    }
}
