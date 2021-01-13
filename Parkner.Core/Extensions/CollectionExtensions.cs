using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Parkner.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> PorCada<T>(this IEnumerable<T> coleccion, Action<T> accion)
        {
            if ((coleccion == null) || (accion == null)) return null;
            IEnumerable<T> porCada = coleccion.ToList();
            foreach (T articulo in porCada) accion(articulo);
            return porCada;
        }

        public static void PorCada<T>(this IEnumerable<T> coleccion, Action<T, int> accion)
        {
            if ((coleccion == null) || (accion == null)) return;
            int contador = 0;
            foreach (T articulo in coleccion)
            {
                accion(articulo, contador);
                ++contador;
            }
        }

        public static void PorCada<T>(this IList<T> coleccion, Action<T, bool> accion)
        {
            if ((coleccion == null) || (accion == null)) return;
            int contador = 0;
            foreach (T articulo in coleccion)
            {
                accion(articulo, contador >= (coleccion.Count - 1));
                ++contador;
            }
        }

        public static IEnumerable<T> PorCada<T>(this IEnumerable coleccion, Action<T> accion) => coleccion.Cast<T>().PorCada(accion);

        public static IEnumerable<RT> PorCada<T, RT>(this IEnumerable<T> coleccion, Func<T, RT> funcion)
        {
            List<RT> lista = new List<RT>();
            foreach (T articulo in coleccion)
            {
                RT obj = funcion(articulo);
                if (obj != null) lista.Add(obj);
            }
            return lista;
        }
    }
}
