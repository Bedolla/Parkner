using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using Parkner.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Parkner.Web.Services
{
    public interface IServicioEstacionamientos
    {
        Task<ListaPaginada<Estacionamiento>> ObtenerTodosAsync(PaginacionPeticion modelo);
        Task<ListaPaginada<Estacionamiento>> ObtenerDeAsync(PaginacionPeticion modelo);
        Task<Estacionamiento> ObtenerAsync(string id);
        Task CrearAsync(Estacionamiento modelo);
        Task EditarAsync(Estacionamiento modelo);
        Task BorrarAsync(string id);
    }

    public class ServicioEstacionamientos : IServicioEstacionamientos
    {
        public ServicioEstacionamientos
        (
            ILogger<ServicioEstacionamientos> registro,
            IConfiguration configuration,
            HttpClient cliente
        )
        {
            this.Registro = registro;
            this.Configuracion = configuration;
            this.Cliente = cliente;
        }

        private ILogger<ServicioEstacionamientos> Registro { get; }

        private IConfiguration Configuracion { get; }

        private HttpClient Cliente { get; }

        public async Task<ListaPaginada<Estacionamiento>> ObtenerTodosAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);

            ListaPaginada<Estacionamiento> estacionamientos = await this.Cliente.PeticionGetAsync<ListaPaginada<Estacionamiento>>("Estacionamientos".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()},
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor}
            }));

            if (estacionamientos.Respuesta is null || estacionamientos.Respuesta.Mensaje.Equals(String.Empty)) return estacionamientos;

            this.Registro.LogError($"Error obteniendo estacionamientos paginados: {estacionamientos.Respuesta.Mensaje}");
            throw new Exception(estacionamientos.Respuesta.Mostrar ? estacionamientos.Respuesta.Mensaje : "Error obteniendo estacionamientos");
        }

        public async Task<ListaPaginada<Estacionamiento>> ObtenerDeAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);

            ListaPaginada<Estacionamiento> estacionamientos = await this.Cliente.PeticionGetAsync<ListaPaginada<Estacionamiento>>("Estacionamientos/De".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"Id", modelo.Id},
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()},
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor}
            }));

            if (estacionamientos.Respuesta is null || estacionamientos.Respuesta.Mensaje.Equals(String.Empty)) return estacionamientos;

            this.Registro.LogError($"Error obteniendo estacionamientos paginados del responsable {modelo.Id}: {estacionamientos.Respuesta.Mensaje}");
            throw new Exception(estacionamientos.Respuesta.Mostrar ? estacionamientos.Respuesta.Mensaje : "Error obteniendo estacionamientos");
        }

        public async Task<Estacionamiento> ObtenerAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);

            Estacionamiento estacionamiento = await this.Cliente.PeticionGetAsync<Estacionamiento>($"Estacionamientos/{id}");

            if (estacionamiento.Respuesta is null || estacionamiento.Respuesta.Mensaje.Equals(String.Empty)) return estacionamiento;

            this.Registro.LogError($"Error obteniendo estacionamiento: {estacionamiento.Respuesta.Mensaje}");
            throw new Exception(estacionamiento.Respuesta.Mostrar ? estacionamiento.Respuesta.Mensaje : "Error obteniendo estacionamiento");
        }

        public async Task CrearAsync(Estacionamiento modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);

            Respuesta respuesta = await this.Cliente.PeticionPostAsync<Respuesta>("Estacionamientos", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error creando estacionamiento: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error creando estacionamiento");
        }

        public async Task EditarAsync(Estacionamiento modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);

            Respuesta respuesta = await this.Cliente.PeticionPutAsync<Respuesta>("Estacionamientos", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error editando estacionamiento: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error editando estacionamiento");
        }

        public async Task BorrarAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);

            Respuesta respuesta = await this.Cliente.PeticionDeleteAsync<Respuesta>($"Estacionamientos/{id}");

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error borrando estacionamiento: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error borrando estacionamiento");
        }
    }
}
