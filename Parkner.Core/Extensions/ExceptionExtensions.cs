using System;

namespace Parkner.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ObtenerMensaje(this Exception excepcion)
        {
            while (excepcion.InnerException != null) excepcion = excepcion.InnerException;
            return excepcion.Message;
        }
    }
}