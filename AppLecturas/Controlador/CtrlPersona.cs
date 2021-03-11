using AppLecturas.Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppLecturas.Controlador
{
    //clase para interactuar entre la interfaz de usuario y la tabla ClsPersona de la base de datos.
    public class CtrlPersona:CtrlBase
    {
        string Url;
        //método para crear la variable cliente que realizará la conexión al servidor usando el protocolo http
       
        
        //método asíncrono que devuelve un objeto enumerable(lista) de tipo clspersona filtrado por cédula, del paquete modelo
        public async Task<IEnumerable<ClsPersona>> ConsultarCi(string Ci)
        {
            try
            {
                return await App.Database.GetPersonaAsync(Ci);
            }
            catch
            {
                return Enumerable.Empty<ClsPersona>();//devuelve una lista vacía
            }
        }
        //método asíncrono que devuelve un objeto enumerable(lista) de tipo clspersona filtrado por Id, del paquete modelo
        public async Task<IEnumerable<ClsPersona>> ConsultarId(int Id)
        {
            try
            {
                return await App.Database.GetPersonaAsync(Id);
            }
            catch
            {
                return Enumerable.Empty<ClsPersona>();//devuelve una lista vacía
            }
        }
        //método asíncrono que devuelve un listado de Personas(Abonados) que aún no han sido sincronizados entre la base local y la remota
        private async Task<IEnumerable<ClsPersona>> GetNuevos()
        {
            try
            {
                List<ClsPersona> ListPersonas = await App.Database.GetPersonaAsync();//consulta de los abonados almacenados
                //en la base de datos local
                string StrIds = "";////varible tipo cadena para guardar los Id existentes en local
                if (ListPersonas.Count > 0)//si el listado de abonados es mayor que cero
                {
                    foreach (ClsPersona item in ListPersonas)
                    {
                        StrIds = StrIds + item.Id + ",";//se arma una cadena de Ids separado por coma(,)
                    }
                    StrIds = StrIds.Substring(0, StrIds.Length - 1);
                }
                else
                    StrIds = "0";//si no hay datos asigno el valor 0 a la cadena 
                //se define la url a la que apunta la petición, indicando el script srvabonados.php que recibe como parametro 
                //la cadena de ids ya registrados
                Url = Servidor + "srvabonados.php" +
                    "?StrIds=" + StrIds;
                //creación de un nuevo objeto Httpclient para hacer la solicitud al servidor remoto
                HttpClient client = getCliente();
                //ejecuta la petición Get al servidor remoto, pasando la url como parámetro
                var resp = await client.GetAsync(Url);
                if (resp.IsSuccessStatusCode)//si el codigo devuelto es satisfactorio
                {
                    string content = await resp.Content.ReadAsStringAsync();//se lee el contenido de la respuesta del servidor
                    return JsonConvert.DeserializeObject<IEnumerable<ClsPersona>>(content);// transforma el contenido de respuesta
                    //de formato json a listado de objetos de la clase ClsPersona
                }
            }
            catch
            {
                return Enumerable.Empty<ClsPersona>();//devuelve una lista vacía
            }           
            return Enumerable.Empty<ClsPersona>();//devuelve una lista vacía
        }
        public async Task<bool> SincronizarAsync()//método para sincronizar abonados entre la base local y la remota
        {
            try
            {
                var Consulta = await GetNuevos();//consulta los abonados nuevos
                if (Consulta != null)//si la consulta tiene datos
                {
                    foreach (ClsPersona item in Consulta)//recorrer la consulta
                    {
                        try
                        {
                            await App.Database.SavePersonaAsync(item);//almacenar cada objeto en la base de datos local
                        }
                        catch (Exception x)
                        {
                            Console.Out.Write(x.Message);
                        }


                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        public async Task<IEnumerable<ClsPersona>> Consultar()//este metodo es para consultar todos los abonados
        {
            try
            {
                return await App.Database.GetPersonaAsync();//con el metodo get me trae  de la base de datos todo los abonados 
            }
            catch
            {
                return Enumerable.Empty<ClsPersona>();//devuelve una lista vacía
            }
        }
    }
}
