using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Plugin.Connectivity;//contine clases para trabajar con la conectividad a internet
using System.Net.Http;
using System.Net.Http.Headers;
using AppLecturas.Modelo;

namespace AppLecturas.Controlador
{
    //clase base de la que herdarán el resto de clases del espacio de nombres Controlador
    public class CtrlBase
    {
        public string Servidor { get; set; }//Propiedad para manejar la URL del servidor
        protected HttpClient getCliente()
        {
            HttpClient client = new HttpClient();//declaramos una variable de tipo httpCliente y la instanciamos
            client.DefaultRequestHeaders.Add("Accept", "application/json");// en esta variable agregamos en el encabezado para eceptar formato Json
            client.DefaultRequestHeaders.Add("Connection", "close");//agregamos en el encabezado para cerrar sesión
            var authData = string.Format("{0}:{1}", MiUsuario.Email, MiUsuario.Password);//declaramos una variable para enviar los datos de email y contraseña del usuario
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));//se convierte a texto de base 64, es una forma de representar los datos encryptados 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);//declarandome una variable, que es de la clase utenticacio
            return client;
        }
        public ClsUsuario MiUsuario { get; set; }//declarando una propiedad de mi clase control base de tipo cls usuario
        public CtrlBase()//constructor de la clase
        {
            Servidor = "https://tesisingresolecturas.000webhostapp.com/api_rest/";//asignar URL donde están alojados los archivosde la apirest
            //Servidor = "http://localhost/api_rest/";
        }
        public bool Esta_Conectado()//devuelve verdadero o falso
        {
            if (CrossConnectivity.Current.IsConnected)//consulta si el dispositivo tiene conección a internet, se invoca a la clases que pertenece CrossConectivity
                return true;//verdadero en caso de ser correcta la conexión
            else 
                return false;//falso si no hay conexión
        }
    }
}
