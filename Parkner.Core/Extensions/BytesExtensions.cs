using Parkner.Core.Constants;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Parkner.Core.Extensions
{
    public static class BytesExtensions
    {
        public static async Task<bool> SubirFotoAsync(this byte[] foto, string nombrDelArchivo, string rol = Roles.Cliente)
        {
            try
            {
                HttpContent contenido = new StreamContent(new MemoryStream(foto));
                contenido.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") {Name = "foto", FileName = nombrDelArchivo};
                contenido.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                HttpClient cliente = new HttpClient {BaseAddress = new Uri(Uris.Api)};
                using MultipartFormDataContent formulario = new MultipartFormDataContent {contenido};
                return (await cliente.PostAsync($"Fotos/{rol}s", formulario)).IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
