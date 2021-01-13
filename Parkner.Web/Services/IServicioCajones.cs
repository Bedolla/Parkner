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
    public interface IServicioCajones
    {
        Task<ListaPaginada<Cajon>> ObtenerTodosAsync(PaginacionPeticion modelo);
        Task<ListaPaginada<Cajon>> ObtenerDeAsync(PaginacionPeticion modelo);
        Task<Cajon> ObtenerAsync(string id);
        Task CrearAsync(Cajon modelo);
        Task EditarAsync(Cajon modelo);
        Task BorrarAsync(string id);
    }

    public class ServicioCajones : IServicioCajones
    {
        public ServicioCajones
        (
            HttpClient cliente,
            ILogger<ServicioCajones> registro,
            IConfiguration configuration
        )
        {
            this.Cliente = cliente;
            this.Registro = registro;
            this.Configuracion = configuration;
        }

        private IConfiguration Configuracion { get; }
        private HttpClient Cliente { get; }
        private ILogger<ServicioCajones> Registro { get; }

        public async Task<ListaPaginada<Cajon>> ObtenerTodosAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            ListaPaginada<Cajon> cajones = await this.Cliente.PeticionGetAsync<ListaPaginada<Cajon>>("Cajones".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()},
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor}
            }));

            if (cajones.Respuesta is null || cajones.Respuesta.Mensaje.Equals(String.Empty)) return cajones;

            this.Registro.LogError($"Error obteniendo cajones paginados: {cajones.Respuesta.Mensaje}");
            throw new Exception(cajones.Respuesta.Mostrar ? cajones.Respuesta.Mensaje : "Error obteniendo cajones");
        }

        public async Task<ListaPaginada<Cajon>> ObtenerDeAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            ListaPaginada<Cajon> cajones = await this.Cliente.PeticionGetAsync<ListaPaginada<Cajon>>("Cajones/De".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"Id", modelo.Id},
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()},
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor}
            }));

            if (cajones.Respuesta is null || cajones.Respuesta.Mensaje.Equals(String.Empty)) return cajones;

            this.Registro.LogError($"Error obteniendo cajones paginados del responsable {modelo.Id}: {cajones.Respuesta.Mensaje}");
            throw new Exception(cajones.Respuesta.Mostrar ? cajones.Respuesta.Mensaje : "Error obteniendo cajones");
        }

        public async Task<Cajon> ObtenerAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Cajon cajon = await this.Cliente.PeticionGetAsync<Cajon>($"Cajones/{id}");

            if (cajon.Respuesta is null || cajon.Respuesta.Mensaje.Equals(String.Empty)) return cajon;

            this.Registro.LogError($"Error obteniendo cajón: {cajon.Respuesta.Mensaje}");
            throw new Exception(cajon.Respuesta.Mostrar ? cajon.Respuesta.Mensaje : "Error obteniendo cajón");
        }

        public async Task CrearAsync(Cajon modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPostAsync<Respuesta>("Cajones", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error creando cajón: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error creando cajón");
        }

        public async Task EditarAsync(Cajon modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPutAsync<Respuesta>("Cajones", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error editando cajón: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error editando cajón");
        }

        public async Task BorrarAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionDeleteAsync<Respuesta>($"Cajones/{id}");

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error borrando cajón: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error borrando cajón");
        }
    }
}
