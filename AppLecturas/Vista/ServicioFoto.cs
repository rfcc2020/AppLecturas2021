using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//clase para manejar la cámara y la galería 
namespace XamarinCamara.Servicios
{
    public class ServicioFoto
    {
        public ServicioFoto()
        {
            CrossMedia.Current.Initialize();
        }

        private static ServicioFoto m_Instancia;

        public static ServicioFoto Instancia
        {
            get
            {
                if (m_Instancia == null)
                {
                    m_Instancia = new ServicioFoto();
                }

                return m_Instancia;
            }
        }

        public async Task<MediaFile> SeleccionarFotoAsync()
        {
            MediaFile foto = null;
            try
            {

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    return null;
                }
                else
                {
                    foto = await CrossMedia.Current.PickPhotoAsync();
                }

            }
            catch (TaskCanceledException)
            {
                foto = null;
            }
            return foto;
        }

        public async Task<MediaFile> CapturarFotoAsync()
        {
            MediaFile foto = null;
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    return null;
                }
                else
                {
                    foto = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
                }

            }
            catch (TaskCanceledException)
            {
                foto = null;
            }
            return foto;
        }
    }
}
