using AppLecturas.Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLecturas.Controlador;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;

namespace AppLecturas.Vista
{
    //clase que maneja la vista menú
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagMenu : ContentPage
    {
        private ClsUsuario ObjMiUsuario;//variable local usuario autenticado
        public PagMenu()//construcror de la clase pagMenu
        {
            InitializeComponent();
            this.ObjMiUsuario = App.Current.Properties["ObjUsuario"] as ClsUsuario;//recuperar objeto guardado en propieades de la aplicación
            TxtUsuario.Text = ObjMiUsuario.Name;//mostrar en caja de texto el nombre de la persona
            TxtPerfil.Text = ObjMiUsuario.Role;//mostrar el nombre del perfil en una caja de texto
            TxtSector.Text = ObjMiUsuario.Sector;//mostrar el sector asignado al usuario
            listView.IsVisible = false;

        }
        //metodo que se ejecuta cuando se muestra la interfaz
        protected override async void OnAppearing()
        {
            base.OnAppearing();//es unmetodo que esta definido en los conten page, es porque vamos a personalizar
            CtrlLectura ObjCtrlLectura = new CtrlLectura();//declaramos la variable y la  instanciamos de la clase CtrlLectura

            try
            {
                // cada vez que se muestre el menú se busca lecturas para sincronizar
                if (ObjCtrlLectura.Esta_Conectado())
                {
                    await SincronizarLecturasAsync();//traer lecturas del servidor remoto
                    ObjCtrlLectura.MiUsuario = ObjMiUsuario;//cargamos los datos del usuario para que autentique.
                    var StrMensaje = await ObjCtrlLectura.Sincronizar();//enviar lecturas al servidor remoto.
                    TxtConectado.Text = "SI";
                    TxtSincronizacion.Text = StrMensaje;
                }
                else
                {
                    TxtConectado.Text = "NO";
                    TxtSincronizacion.Text = "";
                }

                }
                catch (Exception ex)
                {
                    TxtSincronizacion.Text = ex.Message;
                }

        }
        //controlador del botón cerrar evento clic
        private async void ButCerrarSesion_Clicked(object sender, EventArgs e)
        {
            CtrlUsuario ControlUsuario = new CtrlUsuario();
            await ControlUsuario.EliminarUsuarioActualAsync();//es para eliminar el usuario el momento que cierre sesión
            App.Current.Logout();
        }
        //maneja la seleccion de un medidor del listado para crear una nueva lectura
        private async void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                CtrlMedidor Manager = new CtrlMedidor();//declarando una variable de la clase control medidor e intanciandola
                Object ObjFila = e.SelectedItem;//asignar el objeto seleccionado a la variable de tipo object
                var json = JsonConvert.SerializeObject(ObjFila);//declaramos una variable json, y serializo el objeto fila
                ClsMedidor ObjMedidor = JsonConvert.DeserializeObject<ClsMedidor>(json);//transformamos ese json con jsonConver a un cls medidor
                var consulta = await Manager.Consultar(ObjMedidor.Id);//en esta variable vamos a cargar lo que me trae del metodo consultar de la clase medidor por id
                ObjMedidor = consulta.First();//aqui tenemos todos los datos cargados de mi clase medidor
                CtrlLectura ObjCtrlLectura = new CtrlLectura();//declamos una varible de tipo ctrllectura e intanciamos para usar sus metdos
                var LecturaMes = await ObjCtrlLectura.GetLecturaMedidorAsync(DateTime.Today, ObjMedidor.Id);
                if (LecturaMes==null)
                    await ((NavigationPage)this.Parent).PushAsync(new PagIngresoLectura(ObjMedidor, true));//mostrar el formulario Ingresos de lecturas con los datos cargados para modificar o eliminar
                else
                {
                    var resp = await DisplayAlert("Mensaje", "Desea Modificar", "si", "no");
                    if (resp)
                    {
                        await ((NavigationPage)this.Parent).PushAsync(new PagIngresoLectura(LecturaMes, "edit"));//mostrar la vista para modificar una lectura con los datos cargados
                    }
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Mensaje", ex.Message, "ok");
            }
        }
        //maneja boton consultar medidores del sector asignadoal usuario
        private async void ButConsultaMedidores_ClickedAsync(object sender, EventArgs e)
        {
            CtrlMedidor Manager = new CtrlMedidor();
            try
            {
                await SincronizarMedidoresAsync();
                CtrlMedidor ObjMedidor = new CtrlMedidor();
                CtrlPersona ObjPersona = new CtrlPersona();
                var medidores = await ObjMedidor.Consultar(ObjMiUsuario.Sector);
                var personas = await ObjPersona.Consultar();

                var result = from med in medidores
                             join per in personas on med.Persona_id equals per.Id
                             select new
                             {
                                 med.Id,
                                 med.Codigo,
                                 per.Nombre,
                                 per.Apellido,
                                 med.Numero,
                                 med.Marca,
                                 med.Modelo,
                                 med.Sector
                             };
                listView.ItemsSource = result;
                listView.IsVisible = true;
            }
            catch(Exception ex)
            {
                await DisplayAlert("Mensaje", ex.Message, "ok");
            }
        }

        protected async Task<bool> SincronizarMedidoresAsync()//sincroniza medidores asignado al usuario de ese sector
        {
            try
            {
                CtrlMedidor ObjCtrlMedidor = new CtrlMedidor();
                ObjCtrlMedidor.MiUsuario = ObjMiUsuario;//cargamos los datos del usuario para que autentique.
                bool IsValid = await ObjCtrlMedidor.SincronizarAsync();
                return IsValid;
            }
            catch
            {
                return false;
            }
        }
        protected async Task<bool> SincronizarLecturasAsync()//sincroniza lecturas
        {
            try
            {
                CtrlLectura ObjCtrlLectura = new CtrlLectura();//instanciamo con un objeto de la clase control lecturas
                ObjCtrlLectura.MiUsuario = ObjMiUsuario;//cargamos los datos del usuario para que autentique.
                bool IsValid = await ObjCtrlLectura.SincronizarAsync();
                return IsValid;
            }
            catch
            {
                return false;
            }
        }
        //boton que lleva a la interfaz listado de lecturas
        private void ButConsultarLectura_Clicked(object sender, EventArgs e)
        {
            ((NavigationPage)this.Parent).PushAsync(new PagLecturas());
        }
    }
}