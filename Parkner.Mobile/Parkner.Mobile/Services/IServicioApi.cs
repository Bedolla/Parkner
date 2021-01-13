using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Parkner.Mobile.Services
{
    public interface IServicioApi
    {
        Task<bool> SubirFotoAsync(Stream foto, string nombrDelArchivo);
    }

    public class ServicioApi : IServicioApi
    {
        public async Task<bool> SubirFotoAsync(Stream foto, string nombrDelArchivo)
        {
            HttpContent contenido = new StreamContent(foto);
            contenido.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") {Name = "foto", FileName = nombrDelArchivo};
            contenido.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            try
            {
                HttpClient cliente = new HttpClient();
                using MultipartFormDataContent formulario = new MultipartFormDataContent {contenido};
                return (await cliente.PostAsync("http://parknerapi.bedol.la/api/fotos/", formulario)).IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
