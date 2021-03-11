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
    //clase para interactuar entre la interfaz de usuario y la tabla ClsMedidor de la base de datos.
    public class CtrlMedidor:CtrlBase
    {
        string Url;
        //método para crear la variable cliente que realizará la conexión al servidor usando el protocolo http
        
        //método asíncrono que devuelve un objeto enumerable(lista) de tipo clsmedidor del paquete modelo
        public async Task<IEnumerable<ClsMedidor>> Consultar(string Sector)
        {
            try
            {
                return await App.Database.GetMedidorAsync(Sector);//consulta los medidores de agua de un determinado sector
            }
            catch
            {
                return Enumerable.Empty<ClsMedidor>();//devuelve una lista vacía }
            }
        }
        //método asíncrono que devuelve un objeto enumerable(lista) de tipo clsmedidor del paquete modelo
        public async Task<IEnumerable<ClsMedidor>> Consultar(int Id)//consulta un registro en la tabla clsmedidor por Id
        {
            try
            {
                return await App.Database.GetMedidorAsync(Id);
            }
            catch
            {
                return Enumerable.Empty<ClsMedidor>();//devuelve una lista vacía }
            }
        }
        //método consulta un un medidor filtrado por el id de persona asignada.
        public async Task<IEnumerable<ClsMedidor>> ConsultarIdPersona(int IdPersona)
        {
            try
            {
                return await App.Database.GetMedidorPersonaAsync(IdPersona);
            }
            catch
            {
                return Enumerable.Empty<ClsMedidor>();//devuelve una lista vacía }
            }
        }
        //método asíncrono que devuelve un listado de Medidores que aún no han sido sincronizados entre la base local y la remota
        private async Task<IEnumerable<ClsMedidor>> GetNuevos()
        {
            try
            {
                List<ClsMedidor> ListMedidores = await App.Database.GetMedidorAsync();//consulta de los medidores almacenados
                //en la base de datos local
                string StrIds = "";//varible tipo cadena para guardar los Id existentes en local
                if (ListMedidores.Count > 0)//si el listado de medidores es mayor que cero
                {
                    foreach (ClsMedidor item in ListMedidores)
                    {
                        StrIds = StrIds + item.Id + ",";//se arma una cadena de Ids separado por coma(,), consuntando los ides que tenemos en nuetras propiedad de la clase medidor
                    }
                    StrIds = StrIds.Substring(0, StrIds.Length - 1);
                }
                else
                    StrIds = "0";//si no hay datos asigno el valor 0 a la cadena 
                //se define la url a la que apunta la petición, indicando el script srvmedidores.php que recibe como parametro 
                //la cadena de ids ya registrados
                Url = Servidor + "srvmedidores.php" +
                    "?StrIds=" + StrIds;//pasando los ides de los medidores que ya estan guardados mi base de datos localmente
                //creación de un nuevo objeto Httpclient para hacer la solicitud al servidor remoto
                HttpClient client = getCliente();
                //ejecuta la petición Get al servidor remoto, pasando la url como parámetro
                var resp = await client.GetAsync(Url);
                if (resp.IsSuccessStatusCode)//si el codigo devuelto es satisfactorio 
                {
                    string content = await resp.Content.ReadAsStringAsync();//se lee el contenido de la respuesta del servidor
                    return JsonConvert.DeserializeObject<IEnumerable<ClsMedidor>>(content);//transforma el contenido de respuesta
                    //de formato json a listado de objetos de la clase ClsMedidor
                }
            }
            catch
            {
                return Enumerable.Empty<ClsMedidor>();//devuelve una lista vacía
            }
            return Enumerable.Empty<ClsMedidor>();//devuelve una lista vacía
        }
        public async Task<bool> SincronizarAsync()//método para sincronizar medidores entre la base local y la remota
        {
            try
            {
                var Consulta = await GetNuevos();//consulta los medidores nuevos 
                if (Consulta != null)//si la consulta tiene datos
                {
                    foreach (ClsMedidor item in Consulta)//recorrer la consulta
                    {
                        await App.Database.SaveMedidorAsync(item);//almacenar cada objeto en la base de datos local
                    }
                    return true;
                }
            }
            catch { return false; }
            return false;
        }
        public async Task<IEnumerable<ClsMedidor>> Consultar()//consulta todos los medidores
        {
            try
            {
                return await App.Database.GetMedidorAsync();//me devuelve todos los medidores de la base de datos
            }
            catch
            {
                return Enumerable.Empty<ClsMedidor>();//devuelve una lista vacía }
            }
        }
    }
}
