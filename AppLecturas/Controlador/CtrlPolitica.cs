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
    public class CtrlPolitica:CtrlBase
    {
        string Url;
        //método para crear la variable cliente que realizará la conexión al servidor usando el protocolo http
       
        
        //método asíncrono que devuelve un objeto enumerable(lista) de tipo clspolitica, del paquete modelo
        public async Task<IEnumerable<ClsPolitica>> Consultar()
        {
            try
            {
                return await App.Database.GetPoliticaAsync();
            }
            catch
            {
                return Enumerable.Empty<ClsPolitica>();//devuelve una lista vacía
            }
        }
        
        //método asíncrono que devuelve un listado de Politicas que aún no han sido sincronizados entre la base local y la remota
        private async Task<IEnumerable<ClsPolitica>> GetNuevos()
        {
            try
            {

                Url = Servidor + "srvpoliticas.php";
                //creación de un nuevo objeto Httpclient para hacer la solicitud al servidor remoto
                HttpClient client = getCliente();
                //ejecuta la petición Get al servidor remoto, pasando la url como parámetro
                var resp = await client.GetAsync(Url);
                if (resp.IsSuccessStatusCode)//si el codigo devuelto es satisfactorio
                {
                    string content = await resp.Content.ReadAsStringAsync();//se lee el contenido de la respuesta del servidor
                    return JsonConvert.DeserializeObject<IEnumerable<ClsPolitica>>(content);// transforma el contenido de respuesta
                    //de formato json a listado de objetos de la clase ClsPersona
                }
            }
            catch
            {
                return Enumerable.Empty<ClsPolitica>();//devuelve una lista vacía
            }           
            return Enumerable.Empty<ClsPolitica>();//devuelve una lista vacía
        }
        public async Task<bool> SincronizarAsync()//método para sincronizar politicas entre la base local y la remota
        {
            try
            {
                var Consulta = await GetNuevos();//consulta las politicas nuevas
                if (Consulta != null)//si la consulta tiene datos
                {
                    await App.Database.DeletePoliticaAsync();
                    foreach (ClsPolitica item in Consulta)//recorrer la consulta
                    {
                        try
                        {
                            await App.Database.SavePoliticaAsync(item);//almacenar cada objeto en la base de datos local
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
       
    }
}
