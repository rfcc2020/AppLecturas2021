using Newtonsoft.Json;
using SQLite;//base de datos sqlite
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Essentials;

namespace AppLecturas.Modelo
{
    //clase que modela la tabla lectura
    public class ClsLectura:ClsBase
    {
        [PrimaryKey, AutoIncrement]//Clave primaria autoincremental
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public float Anterior { get; set; }
        public float Actual { get; set; }
        public float Consumo { get; set; }
        public float Basico { get; set; }
        public float Exceso { get; set; }
        public string Observacion { get; set; }
        public string Estado { get; set; }//0 no sincronizado,1 sincronizado
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public int Medidor_id { get; set; }
        public int User_id { get; set; }
        public int IdServer { get; set; }
        public string StrImagen { get; set; }
        public float CantidadConsumo { get; set; }
        public float ValorConsumo { get; set; }
        public float VaLorExceso { get; set; }
        public float Total { get; set; }
        
        [JsonIgnore]
        [IgnoreDataMember]
        public string Imagen { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public Xamarin.Forms.ImageSource Image//manejar imágenes
        {
            get 
            {
                if(Imagen != null)
                return Xamarin.Forms.ImageSource.FromStream(
                    () => new MemoryStream(Convert.FromBase64String(Imagen)));
                return null;
            }
        }

        public void Calcular()//método para calcular consumo, exceso y valores según la lectura anterior y la lectura actual
        {
            Consumo = Actual - Anterior;
            Basico = ValorConsumo;
            if (Consumo > CantidadConsumo)
            {                
                Exceso = (Consumo - CantidadConsumo) * VaLorExceso;
            }
            else
            {
                Exceso = 0; 
            }
            Total = Basico + Exceso;
        }

        public async void Localizar()//determina la latitud y longitud de la ubicación según el dispositivo
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    this.Latitud = location.Latitude;
                    this.Longitud = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
        
    }
}
