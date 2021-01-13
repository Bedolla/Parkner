using Parkner.Core.Constants;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Parkner.Core.Extensions
{
    public static class StringExtensions
    {
        public static string Encriptar(this string textoNoCifrado)
        {
            byte[] bytesNoCifrados = Encoding.Unicode.GetBytes(textoNoCifrado);
            using (Aes encriptador = Aes.Create())
            {
                Rfc2898DeriveBytes derivador = new Rfc2898DeriveBytes("P1p0ch45", new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                if (encriptador == null) return textoNoCifrado;
                encriptador.Key = derivador.GetBytes(32);
                encriptador.IV = derivador.GetBytes(16);
                using (MemoryStream flujoDeMemoria = new MemoryStream())
                {
                    using (CryptoStream flujoCríptico = new CryptoStream(flujoDeMemoria, encriptador.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        flujoCríptico.Write(bytesNoCifrados, 0, bytesNoCifrados.Length);
                        flujoCríptico.Close();
                    }
                    textoNoCifrado = Convert.ToBase64String(flujoDeMemoria.ToArray());
                }
            }
            return textoNoCifrado;
        }

        public static string Desencriptar(this string textoCifrado)
        {
            byte[] bytesCifrados = Convert.FromBase64String(textoCifrado);
            using (Aes encriptador = Aes.Create())
            {
                Rfc2898DeriveBytes derivador = new Rfc2898DeriveBytes("P1p0ch45", new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                if (encriptador == null) return textoCifrado;
                encriptador.Key = derivador.GetBytes(32);
                encriptador.IV = derivador.GetBytes(16);
                using (MemoryStream flujoDeMemoria = new MemoryStream())
                {
                    using (CryptoStream flujoCríptico = new CryptoStream(flujoDeMemoria, encriptador.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        flujoCríptico.Write(bytesCifrados, 0, bytesCifrados.Length);
                        flujoCríptico.Close();
                    }
                    textoCifrado = Encoding.Unicode.GetString(flujoDeMemoria.ToArray());
                }
            }
            return textoCifrado;
        }

        public static bool NoEsNuloNiVacío(this string cadenaDeCaracteres) => !String.IsNullOrEmpty(cadenaDeCaracteres);

        public static bool EsNuloVacío(this string cadenaDeCaracteres) => String.IsNullOrEmpty(cadenaDeCaracteres);

        public static bool EsNuloVacioEnBlanco(this string cadenaDeCaracteres) => String.IsNullOrWhiteSpace(cadenaDeCaracteres);

        public static bool NoEsNulo(this string cadenaDeCaracteres) => !String.IsNullOrWhiteSpace(cadenaDeCaracteres);

        public static string HaciaMinúsculas(this string cadenaDeCaracteres) => cadenaDeCaracteres.ToLowerInvariant();

        public static string HaciaTítulo(this string cadenaDeCaracteres) => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(cadenaDeCaracteres);

        public static T HaciaEnumeraciónDe<T>(this string nombre) => (T)Enum.Parse(typeof(T), nombre, true);

        /// <summary>
        ///     Compara una cadena de caracteres con otra, bajo la opción seleccionada
        /// </summary>
        /// <param name="comparado">Cadena de caracteres a comprar</param>
        /// <param name="comparador">Cadena de caracteres que compara</param>
        /// <param name="opcion">Opción de comparación</param>
        /// <returns>Si existe o no una equivalencia</returns>
        public static bool Equivale(this string comparado, string comparador, Equivalencia opcion) => String.Compare(comparado, comparador, CultureInfo.CurrentCulture, (CompareOptions)opcion).Equals(0);

        public static string RemoverDiacriticos(this string textoConDiacriticos)
        {
            string palabra = textoConDiacriticos.Normalize(NormalizationForm.FormD);
            StringBuilder textoSinDiacriticos = new StringBuilder();

            palabra.ToList().ForEach(letra =>
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letra) != UnicodeCategory.NonSpacingMark) textoSinDiacriticos.Append(letra);
            });

            return textoSinDiacriticos.ToString().Normalize(NormalizationForm.FormC);
        }

        public static bool EsNulo(this string id) => String.IsNullOrWhiteSpace(id);

        public static string Valor(this string id) => String.IsNullOrWhiteSpace(id) ? null : id;

        public static async Task<bool> BorrarFotoAsync(this string foto)
        {
            try
            {
                HttpClient cliente = new HttpClient { BaseAddress = new Uri(Uris.Api) };
                return (await cliente.PostAsync("Fotos/Borrar", new StringContent(foto))).IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public enum Equivalencia
    {
        IgnorarNada = CompareOptions.None,
        IgnorarAcentos = CompareOptions.IgnoreNonSpace,
        IgnorarMayusculas = CompareOptions.IgnoreCase,
        IgnorarAcentosMayusculas = CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase
    }

}
