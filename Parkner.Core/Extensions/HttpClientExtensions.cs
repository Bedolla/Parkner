using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Parkner.Core.Extensions
{
    public static class HttpClientExtensions
    {
        private static readonly JsonSerializerOptions Opciones = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public static Task<T> PeticionGetAsync<T>(this HttpClient cliente, string uri, object cuerpo = null) => cliente.EnviarJsonAsync<T>(HttpMethod.Get, uri, cuerpo);

        public static Task PeticionPostAsync(this HttpClient cliente, string uri, object cuerpo) => cliente.EnviarJsonAsync(HttpMethod.Post, uri, cuerpo);

        public static Task<T> PeticionPostAsync<T>(this HttpClient cliente, string uri, object cuerpo) => cliente.EnviarJsonAsync<T>(HttpMethod.Post, uri, cuerpo);

        public static Task PeticionPutAsync(this HttpClient cliente, string uri, object cuerpo) => cliente.EnviarJsonAsync(HttpMethod.Put, uri, cuerpo);

        public static Task<T> PeticionPutAsync<T>(this HttpClient cliente, string uri, object cuerpo) => cliente.EnviarJsonAsync<T>(HttpMethod.Put, uri, cuerpo);

        public static Task PeticionDeleteAsync(this HttpClient cliente, string uri, object cuerpo = null) => cliente.EnviarJsonAsync(HttpMethod.Delete, uri, cuerpo);

        public static Task<T> PeticionDeleteAsync<T>(this HttpClient cliente, string uri, object cuerpo = null) => cliente.EnviarJsonAsync<T>(HttpMethod.Delete, uri, cuerpo);

        private static Task EnviarJsonAsync(this HttpClient cliente, HttpMethod metodo, string uri, object cuerpo) => cliente.EnviarJsonAsync<SinRespuesta>(metodo, uri, cuerpo);

        private static async Task<T> EnviarJsonAsync<T>(this HttpClient cliente, HttpMethod metodo, string uri, object cuerpo)
        {
            HttpResponseMessage respuesta = await cliente.SendAsync(new HttpRequestMessage(metodo, uri)
            {
                Content = new StringContent(JsonSerializer.Serialize(cuerpo, HttpClientExtensions.Opciones), Encoding.UTF8, "application/json")
            });

            return (typeof(T) == typeof(SinRespuesta)) || (await respuesta.Content.ReadAsStringAsync()).EsNulo() ? default : JsonSerializer.Deserialize<T>(await respuesta.Content.ReadAsStringAsync(), HttpClientExtensions.Opciones);
        }

        private class SinRespuesta { }
    }
}
