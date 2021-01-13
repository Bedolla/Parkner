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
    public interface IServicioResponsables
    {
        Task<ListaPaginada<Responsable>> ObtenerTodoAsync(PaginacionPeticion modelo);
        Task<Responsable> ObtenerAsync(string id);
        Task CrearAsync(Responsable modelo);
        Task EditarAsync(Responsable modelo);
        Task BorrarAsync(string id);
    }

    public class ServicioResponsables : IServicioResponsables
    {
        public ServicioResponsables
        (
            HttpClient cliente,
            ILogger<ServicioResponsables> registro,
            IConfiguration configuration
        )
        {
            this.Cliente = cliente;
            this.Registro = registro;
            this.Configuracion = configuration;
        }

        private IConfiguration Configuracion { get; }
        private HttpClient Cliente { get; }
        private ILogger<ServicioResponsables> Registro { get; }

        public async Task<ListaPaginada<Responsable>> ObtenerTodoAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            ListaPaginada<Responsable> responsables = await this.Cliente.PeticionGetAsync<ListaPaginada<Responsable>>("Responsables".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()},
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor}
            }));

            if (responsables.Respuesta is null || responsables.Respuesta.Mensaje.Equals(String.Empty)) return responsables;

            this.Registro.LogError($"Error obteniendo empleados paginados: {responsables.Respuesta.Mensaje}");
            throw new Exception(responsables.Respuesta.Mostrar ? responsables.Respuesta.Mensaje : "Error obteniendo responsables");
        }

        public async Task<Responsable> ObtenerAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Responsable respuesta = await this.Cliente.PeticionGetAsync<Responsable>($"Responsables/{id}");

            if (respuesta.Respuesta is null || respuesta.Respuesta.Mensaje.Equals(String.Empty)) return respuesta;

            this.Registro.LogError($"Error editando responsable: {respuesta.Respuesta.Mensaje}");
            throw new Exception(respuesta.Respuesta.Mostrar ? respuesta.Respuesta.Mensaje : "Error editando responsable");
        }

        public async Task CrearAsync(Responsable modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPostAsync<Respuesta>("Responsables", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error creando responsable: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error creando responsable");
        }

        public async Task EditarAsync(Responsable modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPutAsync<Respuesta>("Responsables", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error editando responsable: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error editando responsable");
        }

        public async Task BorrarAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionDeleteAsync<Respuesta>($"Responsables/{id}");

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error borrando responsable: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error borrando responsable");
        }
    }
}
