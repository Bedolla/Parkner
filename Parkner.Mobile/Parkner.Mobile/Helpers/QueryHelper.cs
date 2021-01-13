using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;

namespace Parkner.Mobile.Helpers
{
    public static class QueryHelper
    {
        public static string AgregarCadenaConsulta(this string uri, string nombre, string valor) => uri == null ? throw new ArgumentNullException(nameof(uri)) : nombre == null ? throw new ArgumentNullException(nameof(nombre)) : valor == null ? throw new ArgumentNullException(nameof(valor)) : QueryHelper.AgregarCadenaConsulta(uri, new[] {new KeyValuePair<string, string>(nombre, valor)});

        public static string AgregarCadenaConsulta(this string uri, IDictionary<string, string> cadenaConsulta) => uri == null ? throw new ArgumentNullException(nameof(uri)) : cadenaConsulta == null ? throw new ArgumentNullException(nameof(cadenaConsulta)) : QueryHelper.AgregarCadenaConsulta(uri, (IEnumerable<KeyValuePair<string, string>>)cadenaConsulta);

        private static string AgregarCadenaConsulta(string uri, IEnumerable<KeyValuePair<string, string>> cadenaConsulta)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            if (cadenaConsulta == null) throw new ArgumentNullException(nameof(cadenaConsulta));

            int indiceDeAnclaje = uri.IndexOf('#');
            string uriPorAgregar = uri;
            string textoDeAnclaje = "";

            if (indiceDeAnclaje != -1)
            {
                textoDeAnclaje = uri.Substring(indiceDeAnclaje);
                uriPorAgregar = uri.Substring(0, indiceDeAnclaje);
            }

            int queryIndex = uriPorAgregar.IndexOf('?');
            bool hasQuery = queryIndex != -1;

            StringBuilder sb = new StringBuilder();
            sb.Append(uriPorAgregar);
            foreach (KeyValuePair<string, string> parameter in cadenaConsulta)
            {
                sb.Append(hasQuery ? '&' : '?');
                sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                sb.Append('=');
                sb.Append(UrlEncoder.Default.Encode(parameter.Value));
                hasQuery = true;
            }

            sb.Append(textoDeAnclaje);
            return sb.ToString();
        }
    }
}
