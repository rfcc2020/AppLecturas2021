using AppLecturas.Modelo;
using SQLite;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace AppLecturas.Base
{
    public class Database//Clase para crear y adminsitrar base de datos local(dispositivo)
    {
        readonly SQLiteAsyncConnection _database;//variable para manejar la conección a la base de datos local, que tiene todo

        public Database(string dbPath)//constructor que recibe un parametro de tipo string(la dirección física donde se creará el archivo de base de datos local)
        {
            _database = new SQLiteAsyncConnection(dbPath);//crear un nuevo objeto tipo conección enviando como parametro la dirección física(path)
            _database.CreateTableAsync<ClsUsuario>().Wait();//crear una tabla en la base de datos local según la clase ClsUsuario del paquete Modelo
            _database.CreateTableAsync<ClsPersona>().Wait();//crear una tabla en la base de datos local según la clase ClsPersona(Abonado) del paquete Modelo
            _database.CreateTableAsync<ClsMedidor>().Wait();//crear una tabla en la base de datos local según la clase ClsMedidor del paquete Modelo
            _database.CreateTableAsync<ClsLectura>().Wait();//crear una tabla en la base de datos local según la clase ClsLectura del paquete Modelo
            _database.CreateTableAsync<ClsUsuarioActual>().Wait();//crear una tabla en la base de datos local según la clase ClsPolitica del paqueteModelo
            _database.CreateTableAsync<ClsPolitica>().Wait();//crear una tabla en la base de datos local según la clase ClsPolitica del paqueteModelo
        }
        public Task<int> SavePoliticaAsync(ClsPolitica politi)//método asíncrono que guarda un nuevo registro en la tabla ClsPolitica, recibe como parametro un objeto de la clase ClsPolitica
        {
            return _database.InsertAsync(politi);//invocación al método Insert en la tabla ClsPolitica enviando el objeto de la clase ClsPolitica, devuelve cero si la operación fracasó.
        }

        public Task<int> SavePersonaAsync(ClsPersona person)//método asíncrono que guarda un nuevo registro en la tabla ClsPersona, recibe como parametro un objeto de la clase ClsPersona
        {
            return _database.InsertAsync(person);//invocación al método Insert en la tabla ClsPersona enviando el objeto de la clase ClsPersona, devuelve cero si la operación fracasó.
        }
        public Task<int> SaveUsuarioAsync(ClsUsuario user)//método asíncrono que guarda un nuevo registro en la tabla ClsUsuario, recibe como parametro un objeto de la clase ClsUsuario
        {
            return _database.InsertAsync(user);//invocación al método Insert en la tabla ClsUsuario enviando el objeto de la clase ClsUsuario, devuelve cero si la operación fracasó.
        }
        public Task<int> SaveMedidorAsync(ClsMedidor medidor)//método asíncrono que guarda un nuevo registro en la tabla ClsMedidor, recibe como parametro un objeto de la clase ClsMedidor
        {
            return _database.InsertAsync(medidor);//invocación al método Insert en la tabla ClsMedidor enviando el objeto de la clase ClsMedidor, devuelve cero si la operación fracasó.
        }
        public Task<List<ClsPersona>> GetPersonaAsync()//método asíncrono que devuelve un listado con todos los registros de la tabla ClsPersona(Abonado) de la base de datos local
        {
            return _database.Table<ClsPersona>().ToListAsync();//invocación a método ToListAsync que convierte los registros de la tabla en un objeto lista.
        }
        public Task<List<ClsPersona>> GetPersonaAsync(int Id)//método asíncrono que devuelve un listado con los registros de la tabla ClsPersona(Abonado) de la base de datos local, 
                                                             //recibe un parametro tipo entero
        {
            return _database.Table<ClsPersona>().Where(c => c.Id == Id).ToListAsync();//invoca al método ToListAsync filtrando los registros por Id con el comando Where 
        }
        public Task<List<ClsPersona>> GetPersonaAsync(string Ci)//método asíncrono que devuelve un listado con los registros de la tabla ClsPersona(Abonado) de la base de datos local, 
                                                                //recibe un parametro tipo string
        {
            return _database.Table<ClsPersona>().Where(c => c.Cedula == Ci).ToListAsync();//invoca al método ToListAsync filtrando los registros por número de Cédula con el comando Where
        }
        public Task<List<ClsMedidor>> GetMedidorAsync()//método asíncrono que devuelve un listado con todos los registros de la tabla ClsMedidor de la base de datos local
        {
            return _database.Table<ClsMedidor>().ToListAsync();//invocación a método ToListAsync que convierte los registros de la tabla en un objeto lista.
        }
        public Task<List<ClsMedidor>> GetMedidorAsync(int Id)//método asíncrono que devuelve un listado con los registros de la tabla ClsMedidor de la base de datos local, 
                                                             //recibe un parametro tipo entero
        {
            return _database.Table<ClsMedidor>().Where(c => c.Id == Id).ToListAsync();//invoca al método ToListAsync filtrando los registros por Id con el comando Where 
        }
        public Task<List<ClsMedidor>> GetMedidorAsync(string  Sector)//método asíncrono que devuelve un listado con los registros de la tabla ClsMedidor de la base de datos local, 
                                                                     //recibe un parametro tipo string
        {
            return _database.Table<ClsMedidor>().Where(c => Sector.Contains(c.Sector)).ToListAsync();//invoca al método ToListAsync filtrando los registros por Sector con el comando Where
        }
        public Task<List<ClsMedidor>> GetMedidorPersonaAsync(int IdPersona)//método asíncrono que devuelve un listado con los registros de la tabla ClsMedidor de la base de datos local, 
                                                                           //recibe un parametro tipo entero
        {
            return _database.Table<ClsMedidor>().Where(c => c.Persona_id == IdPersona).ToListAsync();//invoca al método ToListAsync filtrando los registros por Id de Persona con el comando Where
        }
        public Task<List<ClsUsuario>> GetUsuarioAsync()//método asíncrono que devuelve un listado con todos los registros de la tabla ClsUsuario de la base de datos local
        {
            return _database.Table<ClsUsuario>().ToListAsync();//invocación a método ToListAsync que convierte los registros de la tabla en un objeto lista.
        }
        public Task<List<ClsUsuario>> GetUsuarioAsync(int Id)//método asíncrono que devuelve un listado con los registros de la tabla ClsUsuario de la base de datos local, 
                                                             //recibe un parametro tipo entero
        {
            return _database.Table<ClsUsuario>().Where(c => c.Id == Id).ToListAsync();//invoca al método ToListAsync filtrando los registros por Id con el comando Where
        }
        public Task<List<ClsUsuario>> LoginUsuarioAsync(string email)//método asíncrono que devuelve un listado con los registros de la tabla ClsUsuario de la base de datos local, 
                                                                     //recibe un parametro tipo string
        {
            return _database.Table<ClsUsuario>().Where(c => c.Email == email).ToListAsync();//invoca al método ToListAsync filtrando los registros por email con el comando Where
        }
        
        public Task<int> SaveLecturaAsync(ClsLectura lectura)//método asíncrono que guarda un nuevo registro en la tabla ClsLectura, recibe como parametro un objeto de la clase ClsLectura
        {
            return _database.InsertAsync(lectura);//invocación al método Insert en la tabla ClsLectura enviando el objeto de la clase ClsLectura, devuelve cero si la operación fracasó.
        }
        public Task<List<ClsLectura>> GetLecturaAsync()//método asíncrono que devuelve un listado con los registros de la tabla ClsLectura de la base de datos local
        {
            return _database.Table<ClsLectura>().ToListAsync();//invocación a método ToListAsync que convierte los registros de la tabla en un objeto lista.
        }
        public Task<List<ClsLectura>> GetLecturaAsync(int Id)//método asíncrono que devuelve un listado con los registros de la tabla ClsLectura de la base de datos local, 
                                                             //recibe un parametro tipo int
        {
            return _database.Table<ClsLectura>().Where(c => c.Id == Id).ToListAsync();//invoca al método ToListAsync filtrando los registros por Id con el comando Where
        }
        public Task<List<ClsLectura>> GetLecturaMedidorAsync(int IdMedidor)//método asíncrono que devuelve un listado con los registros de la tabla ClsLectura de la base de datos local, 
                                                             //recibe un parametro tipo int
        {
            return _database.Table<ClsLectura>().Where(c => c.Medidor_id == IdMedidor).ToListAsync();//invoca al método ToListAsync filtrando los registros por Id de Medidor con el comando Where
        }
        public Task<int> UpdateLecturaAsync(ClsLectura lectura)//método asíncrono para actualizar un registro ne la tabla ClsLectura de la base de datos local,
            //recibe como parámetro un objeto de la clase ClsLectura
        {
            return _database.UpdateAsync(lectura);//invoca al método asíncrono UpdateAsync que actualiza el registro, respondiendo con 0 si fracazó
        }
       
        public Task<int> DeleteUsuariosAsync()//método asíncrono para eliminar un registro ne la tabla ClsLectura de la base de datos local,
                                                               //recibe como parámetro un objeto de la clase ClsLectura
        {
            return _database.DeleteAllAsync<ClsUsuario>();//invoca al método asíncrono DeleteAsync que actualiza el registro, respondiendo con 0 si fracazó
        }
        public Task<List<ClsLectura>> GetLecturaAsync(string Estado)//método asíncrono que devuelve un listado con los registros de la tabla ClsLectura de la base de datos local, 
                                                                    //recibe un parametro tipo string
        {
            return _database.Table<ClsLectura>().Where(c => c.Estado != Estado).ToListAsync();//invoca al método ToListAsync filtrando los registros por Estado con el comando Where
        }  
        public Task<int> UpdateLecturaAsync(int Id, int IdServ,string StrEstado)//Método asíncrono para actualizar estado de un registro en la tabla ClsLectura
            //luego de sincronizar con el servidor remoto
            //recibe tres parametros 
            //id representa el id del resgistro de la tabla ClsLectura
            //IdServ es el id del registro en la tabla del serrvidor remoto
            //strestado es el nuevo estado del registro (1 sincronizado,0 no sincronizado) 
        {
            return _database.ExecuteScalarAsync<int>("Update ClsLectura set Estado=?,IdServer=? WHERE Id = ?",StrEstado,IdServ,Id);//invoca al método ExecuteScalarAsync,
            //para actualizar el estado (de 0 a 1) el idServer(id que registra el servidor) en el registro que coincida con el Id recibido(Id de lectura)
        }
        public Task<int> UpdateLecturaAsync(int Id, string StrEstado)//Método asíncrono para actualizar estado de un registro en la tabla ClsLectura
                                                                                 //luego de sincronizar con el servidor remoto
                                                                                 //recibe tres parametros 
                                                                                 //id representa el id del resgistro de la tabla ClsLectura
                                                                                 //IdServ es el id del registro en la tabla del serrvidor remoto
                                                                                 //strestado es el nuevo estado del registro (1 sincronizado,0 no sincronizado) 
        {
            return _database.ExecuteScalarAsync<int>("Update ClsLectura set Estado=? WHERE Id = ?", StrEstado, Id);//invoca al método ExecuteScalarAsync,
            //para actualizar el estado (de 0 a 1) el idServer(id que registra el servidor) en el registro que coincida con el Id recibido(Id de lectura)
        }
        public Task<List<ClsPolitica>> GetPoliticaAsync()//método asíncrono que devuelve un listado con todos los registros de la tabla ClsPolitica de la base de datos local
        {
            return _database.Table<ClsPolitica>().ToListAsync();//invocación a método ToListAsync que convierte los registros de la tabla en un objeto lista.
        }
        public Task<int> DeletePoliticaAsync()//método asíncrono para eliminar un registro ne la tabla Clpolitica de la base de datos local,
                                              //recibe como parámetro un objeto de la clase ClPolitica
        {
            return _database.DeleteAllAsync<ClsPolitica>();//invoca al método asíncrono DeleteAsync que actualiza el registro, respondiendo con 0 si fracazó
        }
        public Task<int> SaveUsuarioActualAsync(ClsUsuarioActual usuario)//método asíncrono que guarda un nuevo registro en la tabla ClsUsuarioActual, recibe como parametro un objeto de la clase ClsUsuarioActual
        {
            return _database.InsertAsync(usuario);//invocación al método Insert en la tabla ClsUsuarioActual enviando el objeto de la clase ClsUsuarioActual, devuelve cero si la operación fracasó.
        }
        public Task<int> DeleteUsuariActualAsync()//método asíncrono para eliminar un registro ne la tabla ClsUsuarioActual de la base de datos local,
                                              //recibe como parámetro un objeto de la clase ClsusuarioActual
        {
            return _database.DeleteAllAsync<ClsUsuarioActual>();//invoca al método asíncrono DeleteAsync que actualiza el registro, respondiendo con 0 si fracazó
        }
        public Task<ClsUsuarioActual> GetUsuarioActualAsync()//método asíncrono que devuelve un listado con todos los registros de la tabla ClsUsuarioActual de la base de datos local
        {
            return _database.Table<ClsUsuarioActual>().FirstAsync();//invocación a método ToListAsync que convierte los registros de la tabla en un objeto lista.
        }
    }
}
