using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Parkner.Data.Entities;
using Parkner.Web.Services;
using System;
using System.Threading.Tasks;

namespace Parkner.Web.Helpers.Pages
{
    public interface IApi
    {
        Task ObtenerCredencialesAsync();
    }

    internal class Api : IApi
    {
        public Api
        (
            ILogger<Api> registro,
            IServicioSesion servicioSesion,
            IConfiguration configuracion
        )
        {
            this.Registro = registro;
            this.ServicioSesion = servicioSesion;
            this.Configuracion = configuracion;
        }

        private IConfiguration Configuracion { get; }
        private ILogger<Api> Registro { get; }
        private IServicioSesion ServicioSesion { get; }

        public async Task ObtenerCredencialesAsync()
        {
            if (this.Configuracion["Token"] != "Token") return;

            try
            {
                this.Configuracion["Token"] = (await this.ServicioSesion.ObtenerToken(new Usuario
                {
                    Correo = this.Configuracion["ApiUsuario"],
                    Clave = this.Configuracion["ApiClave"]
                })).Token;
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Error obteniendo token: {excepcion.Message}");
            }
        }
    }
}
