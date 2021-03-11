using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLecturas.Modelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppLecturas.Controlador;
//interfaz que muestra el listado de lecturas almacenadas en el dispositivo
namespace AppLecturas.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagLecturas : ContentPage
    {
        CtrlLectura Manager;//variable de la clase control lecturas objeto de la clase ctrllectura
        ClsUsuario ObjMiUsuario;//declaramos una variable de tipo ClsUsuario

        public PagLecturas()//constructor Paglecturas
        {
            InitializeComponent();//inicializamos los componentes de la clase PagLecturas
            this.ObjMiUsuario = App.Current.Properties["ObjUsuario"] as ClsUsuario;//
            ButSincr.IsVisible = false;//Es para mostrar el botoncito de sincronizar
            listView.IsVisible = false;
            
            Manager = new CtrlLectura();//es una instancia de la clase control lectura para poder utilizar los metodos de esta clase
        }
        //manejador del boton listar
        private async void Button_ClickedAsync(object sender, EventArgs e)//boton para consultar lecturas, es un controlador 
        {
            try
            {
                listView.IsVisible = true;
                CtrlMedidor ObjMedidor = new CtrlMedidor();//declarando una variable de la clase control medidro e intanciandola
                CtrlPersona ObjPersona = new CtrlPersona();//declarando una variable de la clase control persona e intanciandola
                var lecturas = await Manager.Get();
                var medidores = await ObjMedidor.Consultar(ObjMiUsuario.Sector);
                var personas = await ObjPersona.Consultar();

                var result = from lect in lecturas//hacemos una consulta con la variable lect, en la tabla que está cargada de la tabla lecturas
                             join med in medidores on lect.Medidor_id equals med.Id//utilizo el join cuando estoy consultando en varias tablas, 
                             join per in personas on med.Persona_id equals per.Id
                             select new
                             {
                                 lect.Id,
                                 lect.Fecha,
                                 per.Nombre,
                                 per.Apellido,
                                 lect.Anterior,
                                 lect.Actual,
                                 lect.Latitud,
                                 lect.Longitud,
                                 lect.Consumo
                             };
                listView.ItemsSource = result;
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
            }

        }
        //controlador del evento seleccion de una lectura del listado
        private async void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                Object ObjFila = e.SelectedItem;//asignar el objeto seleccionado a la variable obj
                var json = JsonConvert.SerializeObject(ObjFila);
                ClsLectura ObjLectura = JsonConvert.DeserializeObject<ClsLectura>(json);
                var consulta = await Manager.Get(ObjLectura.Id);
                ObjLectura = consulta.First();
                await ((NavigationPage)this.Parent).PushAsync(new PagIngresoLectura(ObjLectura, "ver"));//mostrar la vista ingreso de lectura con los datos cargados
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
            }
        }
        //controlador del botón sincronizar
        private async void Button_Clicked_SincronizarAsync(object sender, EventArgs e)
        {
            try
            {
               var StrMensaje = await Manager.Sincronizar();
               await DisplayAlert("Información", StrMensaje, "ok");
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
            }
        }

        private async void BuscarPorFecha_Clicked(object sender, EventArgs e)
        {
            try
            {
                int mes, año;
                mes = numeromes(Seleccionarmes.SelectedItem.ToString());
                int.TryParse(Seleccionaraño.SelectedItem.ToString(), out año);
                if (mes != 0 && año != 0)
                {
                    listView.IsVisible = true;
                    CtrlMedidor ObjMedidor = new CtrlMedidor();//declarando una variable de la clase control medidor e intanciandola
                    CtrlPersona ObjPersona = new CtrlPersona();//declarando una variable de la clase control persona e intanciandola
                    var lecturas = await Manager.Get();
                    var medidores = await ObjMedidor.Consultar(ObjMiUsuario.Sector);
                    var personas = await ObjPersona.Consultar();

                    var result = from lect in lecturas//hacemos una consulta con la variable lect, en la tabla que está cargada de la tabla lecturas
                                 join med in medidores on lect.Medidor_id equals med.Id//utilizo el join cuando estoy consultando en varias tablas, 
                                 join per in personas on med.Persona_id equals per.Id
                                 where lect.Fecha.Month == mes && lect.Fecha.Year == año
                                 select new
                                 {
                                     lect.Id,
                                     lect.Fecha,
                                     per.Nombre,
                                     per.Apellido,
                                     lect.Anterior,
                                     lect.Actual,
                                     lect.Latitud,
                                     lect.Longitud,
                                     lect.Consumo,
                                     lect.Image
                                 };
                    listView.ItemsSource = result;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
            }

        }
        int numeromes(string mes)
        {
            switch (mes)
            {
                case "Enero":return 1;
                case "Febrero": return 2;
                case "Marzo": return 3;
                case "Abril": return 4;
                case "Mayo": return 5;
                case "Junio": return 6;
                case "Julio": return 7;
                case "Agosto": return 8;
                case "Septiembre": return 9;
                case "Octubre": return 10;
                case "Noviembre": return 11;
                case "Diciembre": return 12;
                default: return DateTime.Today.Month;
            }
        }
    }
}