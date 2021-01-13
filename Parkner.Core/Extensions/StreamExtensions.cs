using Parkner.Core.Constants;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Parkner.Core.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<byte[]> HaciaBytes(this Stream stream)
        {
            byte[] buffer = new byte[32 * 1024];
            using MemoryStream memoryStream = new MemoryStream();
            int read;
            while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0) memoryStream.Write(buffer, 0, read);
            return memoryStream.ToArray();
        }

        public static async Task<string> HaciaBase64(this Stream stream)
        {
            byte[] buffer = new byte[32 * 1024];
            using MemoryStream memoryStream = new MemoryStream();
            int read;
            while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0) memoryStream.Write(buffer, 0, read);
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static async Task<bool> SubirFotoAsync(this Stream foto, string nombrDelArchivo)
        {
            HttpContent contenido = new StreamContent(foto);
            contenido.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "foto", FileName = nombrDelArchivo };
            contenido.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            try
            {
                HttpClient cliente = new HttpClient();
                using MultipartFormDataContent formulario = new MultipartFormDataContent { contenido };
                return (await cliente.PostAsync(Uris.Api, formulario)).IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
