using AppLecturas.Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Threading;

namespace AppLecturas.Controlador
{
    //clase para interactuar entre la interfaz de usuario y la tabla ClsUsuario de la base de datos.
    public class CtrlUsuario:CtrlBase
    {
        string Url;
        //método para crear la variable cliente que realizará la conexión al servidor usando el protocolo http
        
        //método asíncrono que devuelve un listado de Usuarios que aún no han sido sincronizados entre la base local y la remota
        private async Task<IEnumerable<ClsUsuario>> GetNuevos()//este metodo GetNuevos es para traer los que no tengo en mi base de datos local.
        {
            try
            {
                //se define la url a la que apunta la petición, indicando el script srvusuarios.php que recibe como parametro 
                Url = Servidor + "srvusuarios.php" + "?StrIds=0";//traemos todos los usuario, porque los usuarios son cambienates
                //pasamos parametros en http donde tenemos el nobre y el contenido de usuarios en la base local
                //creación de un nuevo objeto Httpclient para hacer la solicitud al servidor remoto
                HttpClient client = getCliente();
                //ejecuta la petición Get al servidor remoto, pasando la url como parámetro
                var resp = await client.GetAsync(Url);//está variable respuesta nos trae los usuarios que no están en mi base de datos local, para sincronizarlos
                
                if (resp.IsSuccessStatusCode)//si el codigo devuelto es satisfactorio es el encabezado de la respuesta correcta
                {
                    string content = await resp.Content.ReadAsStringAsync();//se lee el contenido de la respuesta del servidor
                    return JsonConvert.DeserializeObject<IEnumerable<ClsUsuario>>(content);//transforma el contenido de respuesta
                    //de formato json a listado de objetos de la clase ClsUsuario
                }
                else
                    return Enumerable.Empty<ClsUsuario>();//devuelve una lista vacía
            }
            catch(Exception ex)
            {
                throw new Exception("Error al consultar informacion del origen remoto. Razon: "+ex.Message);//devuelve una lista vacía
            }
        }
        public async Task<bool> SincronizarAsync()//método para sincronizar usuarios entre la base local y la remota
        {
            try
            {
                var Consulta = await GetNuevos();//este metodo GetNuevos consulta todos, traer todos los usuarios mi base de datos local
                if (Consulta != null)//si la consulta tiene datos
                {
                    await App.Database.DeleteUsuariosAsync();//la idea es mantener actualizado los datos, un ejemplo si a algun usuario le asignamos otro sector, por eso tenemos que eliminar para que se vuelva a crear
                    foreach (ClsUsuario item in Consulta)//recorrer la consulta es un listado de objeto de clsUsuario
                    {
                        await App.Database.SaveUsuarioAsync(item);//almacenar cada objeto en la base de datos local
                    }
                    return true;
                }
            }
            catch(Exception ex) {
                throw new Exception("Error al consultar informacion del origen remoto. Razon: " + ex.Message);//devuelve error 
            }

            return false;
        }
        //método asíncrono que devuelve un objeto enumerable(lista) de tipo clsusuario del paquete modelo filtrado por email
        public async Task<IEnumerable<ClsUsuario>> LoginUsr(string email)
        {
            try
            {
                List<ClsUsuario> ObjUsr = await App.Database.LoginUsuarioAsync(email);//busco al usuario que coincida por email
                return ObjUsr;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar informacion del origen remoto. Razon: " + ex.Message);//devuelve error
            }
        }
        public async Task<bool> CrearUsuarioActualAsync(ClsUsuarioActual item)//método para crear el usuario actual en la base local 
        {
            try
            { 
                 await App.Database.SaveUsuarioActualAsync(item);//almacenar cada objeto en la base de datos local
                 return true;    
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar informacion del origen remoto. Razon: " + ex.Message);//devuelve error 
            }
        }
        public async Task<bool> EliminarUsuarioActualAsync()//método para eliminar el usuario en la base local 
        {
            try
            {
                await App.Database.DeleteUsuariActualAsync();//elimina cada objeto en la base de datos local
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar informacion del origen remoto. Razon: " + ex.Message);//devuelve error 
            }
        }
        public async Task<ClsUsuarioActual> GetUsuarioActual()//método para consultar el usuario que está logeado a la base local 
        {
            try
            {
                return await App.Database.GetUsuarioActualAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}
