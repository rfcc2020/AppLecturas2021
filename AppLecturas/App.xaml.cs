using AppLecturas.Interfaces;
using AppLecturas.Vista;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppLecturas
{
    //clase que representa la aplicación

    public partial class App : Application,ILoginManager
    {
        //static ILoginManager ObjLoginManager;//objeto de la interfaz Iloginmanager
        public new static App Current;//variable local que va a representar a la aplicación

        static Base.Database database;//crear una variable de la clase database

        public static Base.Database Database//crea la base de datos local
        {
            get
            {
                if (database == null)//en caso de aún no existir crea la base de datos 
                {
                    database = new Base.Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "baselocal9121.db3"));//creación de la base de datos local
                }
                return database;//retorna cargada con las tablas la base de datos existente o la creada
            }
        }

        public App()//constructor
        {
            InitializeComponent();//inicializa elemento visuales
            Current = this;//asigna aplicación a la variable que me permitira usar los metodos de la interfaz
            var isLoggedIn = Properties.ContainsKey("IsLoggedIn") ? (bool)Properties["IsLoggedIn"] : false;//compara si hay usuario autenticado
            if (isLoggedIn)
                MainPage = new NavigationPage(new PagMenu());//si el usuario ya se ha autenticado muestra el menú principal
            else
                MainPage = new LoginModalPage(this);//si el usuario aún no se ha autenticado muestra la interfaz login
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        //método que muestra la página principal, en este caso la página menú
        public void ShowMainPage()
        {
            MainPage = new NavigationPage(new PagMenu());
        }
        //método que desautentica a un usurio y luego muestra la interfaz login
        public void Logout()
        {
            Properties["IsLoggedIn"] = false;
            MainPage = new LoginModalPage(this);
        }
        //método para cerrar la aplicación
        public void Salir()
        {
            this.Quit();
        }
    }
}
