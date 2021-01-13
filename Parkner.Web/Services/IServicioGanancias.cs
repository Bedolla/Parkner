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
    public interface IServicioGanancias
    {
        Task<ListaPaginada<Ganancia>> ObtenerTodosAsync(PaginacionPeticion modelo);
        Task<ListaPaginada<Ganancia>> ObtenerDeAsync(PaginacionPeticion modelo);
        Task<Ganancia> ObtenerAsync(string id);
        Task CrearAsync(Ganancia modelo);
        Task EditarAsync(Ganancia modelo);
        Task BorrarAsync(string id);
    }

    public class ServicioGanancias : IServicioGanancias
    {
        public ServicioGanancias
        (
            HttpClient cliente,
            ILogger<ServicioGanancias> registro,
            IConfiguration configuration
        )
        {
            this.Cliente = cliente;
            this.Registro = registro;
            this.Configuracion = configuration;
        }

        private IConfiguration Configuracion { get; }
        private HttpClient Cliente { get; }
        private ILogger<ServicioGanancias> Registro { get; }

        public async Task<ListaPaginada<Ganancia>> ObtenerTodosAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            ListaPaginada<Ganancia> ganancias = await this.Cliente.PeticionGetAsync<ListaPaginada<Ganancia>>("Ganancias".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()},
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor}
            }));

            if (ganancias.Respuesta is null || ganancias.Respuesta.Mensaje.Equals(String.Empty)) return ganancias;

            this.Registro.LogError($"Error obteniendo ganancias paginadas: {ganancias.Respuesta.Mensaje}");
            throw new Exception(ganancias.Respuesta.Mostrar ? ganancias.Respuesta.Mensaje : "Error obteniendo ganancias");
        }

        public async Task<ListaPaginada<Ganancia>> ObtenerDeAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            ListaPaginada<Ganancia> ganancias = await this.Cliente.PeticionGetAsync<ListaPaginada<Ganancia>>("Ganancias/De".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"Id", modelo.Id},
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()},
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor}
            }));

            if (ganancias.Respuesta is null || ganancias.Respuesta.Mensaje.Equals(String.Empty)) return ganancias;

            this.Registro.LogError($"Error obteniendo ganancias paginadas del responsable {modelo.Id}: {ganancias.Respuesta.Mensaje}");
            throw new Exception(ganancias.Respuesta.Mostrar ? ganancias.Respuesta.Mensaje : "Error obteniendo ganancias");
        }

        public async Task<Ganancia> ObtenerAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Ganancia ganancia = await this.Cliente.PeticionGetAsync<Ganancia>($"Ganancias/{id}");

            if (ganancia.Respuesta is null || ganancia.Respuesta.Mensaje.Equals(String.Empty)) return ganancia;

            this.Registro.LogError($"Error obteniendo ganancia: {ganancia.Respuesta.Mensaje}");
            throw new Exception(ganancia.Respuesta.Mostrar ? ganancia.Respuesta.Mensaje : "Error obteniendo ganancia");
        }

        public async Task CrearAsync(Ganancia modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPostAsync<Respuesta>("Ganancias", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error creando ganancia: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error creando ganancia");
        }

        public async Task EditarAsync(Ganancia modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPutAsync<Respuesta>("Ganancias", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error editando ganancia: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error editando ganancia");
        }

        public async Task BorrarAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionDeleteAsync<Respuesta>($"Ganancias/{id}");

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error borrando ganancia: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error borrando ganancia");
        }
    }
}
