using AppLecturas.Controlador;
using AppLecturas.Interfaces;
using AppLecturas.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppLecturas.Vista
{
    //clase que maneja la vista login
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagLogin : ContentPage
    {
        ILoginManager Ilm = null;//declaramos un objeto Ilm de la clase IloginManager y se inicializa en nulo
        ClsUsuario ObjUsuario;// declaro la variable de tipo cls usuario
        public PagLogin(ILoginManager Ilm)//constructor recibiendo como parámetro objeto Ilm de clase interfaz Iloginmanager
        {
            InitializeComponent();
            this.Ilm = Ilm;//asignar variable local
        }
       //
       
        public PagLogin()//constructor
        {
            InitializeComponent();
        }
        //metodo que se ejecuta cuando se muestra la interfaz
        protected override async void OnAppearing()//metodo que se ejecuta cuando se va a mostar el contenpage
        {
            base.OnAppearing();
            ClsUsuarioActual UsuarioActual = await BuscarUsuarioActualAsync();
            if (UsuarioActual != null)
            {
                App.Current.Properties["name"] = UsuarioActual.Name;//guardar en propiedades de la aplicación el nombre del usuario
                App.Current.Properties["IsLoggedIn"] = true;//guardar en propiedades de la aplicación el estado como verdadero
                ObjUsuario = new ClsUsuario();
                ObjUsuario.Id = UsuarioActual.Id;
                ObjUsuario.Name = UsuarioActual.Name;
                ObjUsuario.Email = UsuarioActual.Email;
                ObjUsuario.Password = UsuarioActual.Password;
                ObjUsuario.Role = UsuarioActual.Role;
                ObjUsuario.Sector = UsuarioActual.Sector;
                ObjUsuario.Updated_at = UsuarioActual.Updated_at;
                App.Current.Properties["ObjUsuario"] = ObjUsuario;//guardar el objeto usuario en propiedades de la aplicación
                Ilm.ShowMainPage();
            }
        }
        /// Encripta una cadena
        private bool VerificarPassword(string StrEncPassword, string UsrPassword)//metodo que compara el password ingresado por el usuario
        {
            bool isValid = BCrypt.Net.BCrypt.Verify(StrEncPassword, UsrPassword);//str es el valor del password que esta guardado del usuario con Usr es el password que ingresa el usuario
            return isValid;
        }
        //manejador evento clic del botón entrar
        private async void ButEntrar_Clicked(object sender, EventArgs e)
        {
            try
            {
                    if (!string.IsNullOrWhiteSpace(TxtEmail.Text))//Este metodo es de la clase String del paquete de la plataforma .Net, que me valida el email no nulo
                    if (!string.IsNullOrWhiteSpace(TxtPassword.Text))//validar password no nulo
                        if (TxtEmail.TextColor == Color.Green)//validar email con formato correcto
                            if (TxtPassword.Text.Length >= 6)//validar que el password sea mayor o igual a 6 caracteres 
                            {
                                CtrlUsuario ObjCtrlUsuario = new CtrlUsuario();//declaramos una varioable e instanciamos el controlador usuario
                                ObjUsuario = new ClsUsuario();//declaramos uan variable e instanciamos la clase ClsUsuario
                                ObjUsuario.Email = TxtEmail.Text;//asigno la propiedad email del objeto usuario
                                ObjUsuario.Password = TxtPassword.Text;//asigno la propiedad password del objeto usuario
                                bool IsValidSyncUsuarios = await SincronizarUsuariosAsync();
                                if (!IsValidSyncUsuarios)
                                    TxtMsg.Text = "No se ha podido recuperar la información del origen remoto";
                                else
                                    TxtMsg.Text = "Información recuperada correctamente desde el origen remoto";
                                var ConsUsr = await ObjCtrlUsuario.LoginUsr(TxtEmail.Text);//invoca al método login del controlador usuario
                                if (ConsUsr.Count() == 1)//si existe un registro que coincide con el email
                                {
                                    bool PassValido = false;//está variable me ayuda a guardar el resultado que me devuelva el metodo password
                                    foreach (ClsUsuario item in ConsUsr)//recorrer la lista
                                    {
                                        if (VerificarPassword(TxtPassword.Text, item.Password))//invocamos al metodo verificar password verificar password
                                        {
                                            PassValido = true;//cuando se encuentra el password
                                            break;// es para salir del bucle antes de terminar de recorrer
                                        }
                                    }
                                    if (PassValido)//si el password es valido se continua
                                    {
                                        await SincronizarPersonasAsync();
                                        ClsUsuario ObjUsuario = ConsUsr.First();//declaro una variable de la clase usuario, y le asigno el primer usuario del liostado de objetos que se recibio
                                        await DisplayAlert("Mensaje", "Bienvenido", "ok");//mensaje de  bienvenida
                                                                                          //ObjUsuario.ObjPerfil = ConsPerfil.First();//asignar objeto encontrado a campo de objeto usuario
                                        ObjUsuario.Password = TxtPassword.Text;//asignando a la propiedad password el password ingresado
                                        App.Current.Properties["name"] = ObjUsuario.Name;//guardar en propiedades de la aplicación el nombre del usuario
                                        App.Current.Properties["IsLoggedIn"] = true;//guardar en propiedades de la aplicación el estado como verdadero
                                        App.Current.Properties["ObjUsuario"] = ObjUsuario;//guardar el objeto usuario en propiedades de la aplicación
                                        ClsUsuarioActual ObjUsuarioActual = new ClsUsuarioActual//aqlamcenamos el usuario actual en la base de datos local
                                        {
                                            Id = ObjUsuario.Id,
                                            Name = ObjUsuario.Name,
                                            Password = ObjUsuario.Password,
                                            Email = ObjUsuario.Email,
                                            Role = ObjUsuario.Role,
                                            Sector = ObjUsuario.Sector,
                                            Updated_at = ObjUsuario.Updated_at
                                        };
                                        await ObjCtrlUsuario.CrearUsuarioActualAsync(ObjUsuarioActual);
                                        Ilm.ShowMainPage();


                                    }
                                    else
                                        await DisplayAlert("Mensaje", "Datos no encontrados, vuelva a intentar", "ok");
                                }
                                else
                                        await DisplayAlert("Mensaje", "Datos no encontrados, vuelva a intentar", "ok");
                            }
                            else
                                await DisplayAlert("Mensaje", "Debe ingresar minimo 6 caracteres en el password", "ok");
                        else
                            await DisplayAlert("Mensaje", "Email con formato incorrecto", "ok");
                    else
                        await DisplayAlert("Mensaje", "Debe ingresar el password", "ok");
                else
                    await DisplayAlert("Mensaje", "Debe ingresar el email", "ok");
            }
            catch (Exception x) { await DisplayAlert("Mensaje", x.Message, "ok"); }
        }
        private void ButCancelar_Clicked(object sender, EventArgs e)
        {
            TxtEmail.Text = "";
            TxtPassword.Text = "";
        }
        //Metodo para sincronizar los abonados
        protected async Task<bool> SincronizarPersonasAsync()
        {
            try
            {
                CtrlPersona ObjCtrlPersona = new CtrlPersona();
                ObjCtrlPersona.MiUsuario = ObjUsuario;//cargamos los datos del usuario para que autentique.
                if (ObjCtrlPersona.Esta_Conectado())
                    return await ObjCtrlPersona.SincronizarAsync();
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        //sincronizar los usuarios
        protected async Task<bool> SincronizarUsuariosAsync()
        {
            try
            {
                CtrlUsuario ObjCtrlUsuario = new CtrlUsuario();//declarando una variable de la clase control ususario e intanciandola
                ObjCtrlUsuario.MiUsuario = ObjUsuario;//cargamos los datos del usuario para que autentique.
                if (ObjCtrlUsuario.Esta_Conectado())
                    return await ObjCtrlUsuario.SincronizarAsync();//me retorna verdadero o falso, dependiendo del resultado Sincronizar Async
                else return false;
            }
            catch
            {
                return false;
            }
        }
        //buscar usuario logueado
        protected async Task<ClsUsuarioActual> BuscarUsuarioActualAsync()//este metodo es para buscar el usuario que está logeado en la base de datos local
        {
            try
            {
                CtrlUsuario ObjCtrlUsuario = new CtrlUsuario();
                return await ObjCtrlUsuario.GetUsuarioActual();

            }
            catch
            {
                return null;
            }
        }

    }
}