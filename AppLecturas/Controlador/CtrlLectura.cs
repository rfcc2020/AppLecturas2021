using AppLecturas.Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppLecturas.Controlador
{
    //clase para interactuar entre la interfaz de usuario y el modelo de base de datos local y remoto
    public class CtrlLectura : CtrlBase//hereda de ctrlbase
    {
        string Url;
        //método para crear la variable cliente que realizará la conexión al servidor remoto usando el protocolo http
       

   //método para obtener la lectura anterior de un medidor
        public async Task<List<ClsLectura>> ConsultarAnterior(int IdMedidor)
        {
            try
            {
                return await App.Database.GetLecturaMedidorAsync(IdMedidor);//invoca al método de la clase Database
            }
            catch
            {
                return Enumerable.Empty<ClsLectura>() as List<ClsLectura>;//devuelve una lista vacía
            }
        }
        //método para consultar si un medidor ya tiene un registro correspondiente al mes y año actual
        public async Task<ClsLectura> GetLecturaMedidorAsync(DateTime Fecha, int IdMedidor)
        {
            try
            {
                var ListLecturas = await App.Database.GetLecturaMedidorAsync(IdMedidor);//consulta las lecturas que corresponde al id de medidor
                foreach (ClsLectura item in ListLecturas)//recorrer el listado de lecturas
                {
                    if (item.Fecha.Month == Fecha.Month && item.Fecha.Year == Fecha.Year)//si la fecha del registro coincide con el año y mes actual devuelve true y termina el método
                    {
                        return item;
                    }
                }
            }
            catch
            {
                return null;//si hay error devuelve null
            }
            return null;//si no encuentra ningún registro devuelve null
        }
        public async Task<IEnumerable<ClsLectura>> Get()//consultar todas las lecturas listado de lecturas
        {
            try
            {
                return await App.Database.GetLecturaAsync();
            }
            catch
            {
                return Enumerable.Empty<ClsLectura>();//devuelve una lista vacía
            }
        }
        public async Task<IEnumerable<ClsLectura>> GetNoSincronizados()//consultar lecturas con estado 1
        {
            try
            {
                return await App.Database.GetLecturaAsync("1");
            }
            catch
            {
                return Enumerable.Empty<ClsLectura>();//devuelve una lista vacía
            }
        }

        public async Task<string> Sincronizar()//método para sincronizar con servidor remoto
        {
            var ListLecturas = await GetNoSincronizados();//obtener lecturas con estado = 0
            int sinc=0, nsinc = 0;//variables para mostrar resultado sinc=sincronizados, nsinc=no sincronizados
            Url = Servidor + "srvlecturas.php";//armar la url con la dirección del sevidor y el script srvlecturas.php
            HttpClient client = getCliente();//crear un nuevo objeto tipo cliente http
            foreach (ClsLectura item in ListLecturas)//recorrer el listado de lecturas no sincronizadas
            {
                if(item.IdServer==0)//si el id es igual a cero se crea una lectura
                try
                {
                    var formContent = new FormUrlEncodedContent(new[]//armar un formulario con los datos del objeto
                {
                new KeyValuePair<string, string>("Fecha", item.Fecha.Year + "/"+item.Fecha.Month+"/"+item.Fecha.Day),
                new KeyValuePair<string, string>("Anterior", item.Anterior.ToString()),
                new KeyValuePair<string, string>("Actual", item.Actual.ToString()),
                new KeyValuePair<string, string>("Consumo", item.Consumo.ToString()),
                new KeyValuePair<string, string>("Basico", item.Basico.ToString()),
                new KeyValuePair<string, string>("Exceso", item.Exceso.ToString()),
                new KeyValuePair<string, string>("Observacion", item.Observacion.ToString()),
                new KeyValuePair<string, string>("Imagen", item.StrImagen),
                new KeyValuePair<string, string>("Latitud", item.Latitud.ToString()),
                new KeyValuePair<string, string>("Longitud", item.Longitud.ToString()),
                new KeyValuePair<string, string>("Estado", item.Estado),
                new KeyValuePair<string, string>("Medidor_id", item.Medidor_id.ToString()),
                new KeyValuePair<string, string>("User_id", item.User_id.ToString()),
                new KeyValuePair<string, string>("Created_at", item.Created_at.Year+"/"+item.Created_at.Month+"/"+item.Created_at.Day),
                new KeyValuePair<string, string>("Updated_at", item.Updated_at.Year+"/"+item.Updated_at.Month+"/"+item.Updated_at.Day),
            });
                    var response = await client.PostAsync(Url+"?id=0", formContent);//enviar la petición http al servidor remoto y recoger el resultado en la variable response
                    if (response.IsSuccessStatusCode)//si la respuesta viene con código correcto 
                    {
                        var json = await response.Content.ReadAsStringAsync();//se lee el contenido de la respuesta del servidor
                            ClsLectura result = JsonConvert.DeserializeObject<ClsLectura>(json);//result objeto de la clase clslectura 
                        await App.Database.UpdateLecturaAsync(item.Id, result.IdServer, "1");//actualizar el registro de la tabla clslectura
                        sinc++;//incremento 
                    }
                    else
                        nsinc++;
                }
                catch { nsinc++; }
            else
                try
                {
                    var formContent = new FormUrlEncodedContent(new[]//armar un formulario con los datos del objeto
                {
                new KeyValuePair<string, string>("Fecha", item.Fecha.Year + "/"+item.Fecha.Month+"/"+item.Fecha.Day),
                new KeyValuePair<string, string>("Anterior", item.Anterior.ToString()),
                new KeyValuePair<string, string>("Actual", item.Actual.ToString()),
                new KeyValuePair<string, string>("Consumo", item.Consumo.ToString()),
                new KeyValuePair<string, string>("Basico", item.Basico.ToString()),
                new KeyValuePair<string, string>("Exceso", item.Exceso.ToString()),
                new KeyValuePair<string, string>("Observacion", item.Observacion.ToString()),
                new KeyValuePair<string, string>("Imagen", item.StrImagen),
                new KeyValuePair<string, string>("Latitud", item.Latitud.ToString()),
                new KeyValuePair<string, string>("Longitud", item.Longitud.ToString()),
                new KeyValuePair<string, string>("Estado", item.Estado),
                new KeyValuePair<string, string>("Medidor_id", item.Medidor_id.ToString()),
                new KeyValuePair<string, string>("User_id", item.User_id.ToString()),
                new KeyValuePair<string, string>("Created_at", item.Created_at.Year+"/"+item.Created_at.Month+"/"+item.Created_at.Day),
                new KeyValuePair<string, string>("Updated_at", item.Updated_at.Year+"/"+item.Updated_at.Month+"/"+item.Updated_at.Day),
                new KeyValuePair<string, string>("idServer", item.IdServer.ToString()),
            });
                    var response = await client.PostAsync(Url+"?id="+item.IdServer, formContent);//enviar la petición http al servidor remoto y recoger el resultado en la variable response
                    if (response.IsSuccessStatusCode)//si la respuesta viene con código correcto 
                    {
                        var json = await response.Content.ReadAsStringAsync();//recibe la respuesta en formato json
                            if (json == "ok")
                            {
                                await App.Database.UpdateLecturaAsync(item.Id, "1");//actualizar el registro de la tabla clslectura
                                sinc++;//incremento
                            }
                            else nsinc++;
                    }
                    else
                        nsinc++;
                }
                catch (Exception x) { nsinc++; }
        }
                
            return "Lecturas Sincronizadas: "+sinc+"  Lecturas No sincronizadas: "+nsinc;//devuelve resultado lecturass sincronizadas y no sincronizadas
        }
      
        //guardar una nueva lectura
        public async Task<string> SaveAsync(ClsLectura ObjLectura)//recibe un objeto de la clase clslectura
        {
            try
            {
                ObjLectura.Estado = "0";//se guardo por defecto con el estado 0, porque no está sincronizado
                ObjLectura.Calcular();//llamada a método calcular
                await App.Database.SaveLecturaAsync(ObjLectura);//
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        //actualizar una lectura
        public async Task<string> UpdateAsync(ClsLectura ObjLectura)
        {
            try
            {
                ObjLectura.Estado = "2";
                await App.Database.UpdateLecturaAsync(ObjLectura);
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private async Task<IEnumerable<ClsLectura>> GetNuevos()
        {
            try
            {
                List<ClsLectura> ListLecturas = await App.Database.GetLecturaAsync();//consulta de las lecturas almacenadas
                //en la base de datos local
                string StrIds = "";//varible tipo cadena para guardar los Id existentes en local
                if (ListLecturas.Count > 0)//si el listado de lecturas es mayor que cero
                {
                    foreach (ClsLectura item in ListLecturas)
                    {
                        StrIds = StrIds + item.IdServer + ",";//se arma una cadena de Ids separado por coma(,), consuntando los is server que tenemos en nuetras propiedad de la clase lectura
                    } 
                    StrIds = StrIds.Substring(0, StrIds.Length - 1);
                }
                else
                    StrIds = "0";//si no hay datos asigno el valor 0 a la cadena 
                //se define la url a la que apunta la petición, indicando el script srvlecturas.php que recibe como parametro 
                //la cadena de ids ya registrados
                Url = Servidor + "srvlecturas.php" +
                    "?StrIds=" + StrIds;//pasando los ides de las lecturas que ya estan guardados mi base de datos localmente
                //creación de un nuevo objeto Httpclient para hacer la solicitud al servidor remoto
                HttpClient client = getCliente();
                //ejecuta la petición Get al servidor remoto, pasando la url como parámetro
                var resp = await client.GetAsync(Url);
                if (resp.IsSuccessStatusCode)//si el codigo devuelto es satisfactorio 
                {
                    string content = await resp.Content.ReadAsStringAsync();//se lee el contenido de la respuesta del servidor
                    return JsonConvert.DeserializeObject<IEnumerable<ClsLectura>>(content);//transforma el contenido de respuesta
                    //de formato json a listado de objetos de la clase Clslectura
                }
            }
            catch
            {
                return Enumerable.Empty<ClsLectura>();//devuelve una lista vacía
            }
            return Enumerable.Empty<ClsLectura>();//devuelve una lista vacía
        }
        public async Task<bool> SincronizarAsync()//método para sincronizar lecturas entre la base local y la remota
        {
            try
            {
                var Consulta = await GetNuevos();//consulta las lecturas nuevas 
                if (Consulta != null)//si la consulta tiene datos
                {
                    foreach (ClsLectura item in Consulta)//recorrer la consulta
                    {
                        await App.Database.SaveLecturaAsync(item);//almacenar cada objeto en la base de datos local
                    }
                    return true;
                }
            }
            catch { return false; }
            return false;
        }
        public async Task<IEnumerable<ClsLectura>> Get(int id)//consultar listado de lecturas
        {
            try
            {
                return await App.Database.GetLecturaAsync(id);
            }
            catch
            {
                return Enumerable.Empty<ClsLectura>();//devuelve una lista vacía
            }
        }
        public async Task<IEnumerable<ClsLectura>> GetLecturaConsultarMesAsync(int mes, int año)
        {
            
            try
            {
                var ListLecturas = await App.Database.GetLecturaAsync();//consulta las lecturas que corresponde al id de medidor
                foreach (ClsLectura item in ListLecturas)//recorrer el listado de lecturas
                {
                    if (item.Fecha.Month != mes || item.Fecha.Year != año)//si la fecha del registro coincide con el año y mes actual devuelve true y termina el método
                    {
                        ListLecturas.Remove(item);
     
                    }
                }
                return ListLecturas;
            }
            catch
            {
                return Enumerable.Empty<ClsLectura>();//devuelve una lista vacía
            }
            
        }
    }
}
