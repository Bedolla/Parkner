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
    public interface IServicioHorarios
    {
        Task<ListaPaginada<Horario>> ObtenerTodosAsync(PaginacionPeticion modelo);
        Task<Horario> ObtenerAsync(string id);
        Task CrearAsync(Horario modelo);
        Task EditarAsync(Horario modelo);
        Task BorrarAsync(string id);
    }

    public class ServicioHorarios : IServicioHorarios
    {
        public ServicioHorarios
        (
            HttpClient cliente,
            ILogger<ServicioHorarios> registro,
            IConfiguration configuration
        )
        {
            this.Cliente = cliente;
            this.Registro = registro;
            this.Configuracion = configuration;
        }

        private IConfiguration Configuracion { get; }
        private HttpClient Cliente { get; }
        private ILogger<ServicioHorarios> Registro { get; }

        public async Task<ListaPaginada<Horario>> ObtenerTodosAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            ListaPaginada<Horario> horarios = await this.Cliente.PeticionGetAsync<ListaPaginada<Horario>>("Horarios".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()},
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor}
            }));

            if (horarios.Respuesta is null || horarios.Respuesta.Mensaje.Equals(String.Empty)) return horarios;

            this.Registro.LogError($"Error obteniendo horarios paginados: {horarios.Respuesta.Mensaje}");
            throw new Exception(horarios.Respuesta.Mostrar ? horarios.Respuesta.Mensaje : "Error obteniendo horarios");
        }

        public async Task<Horario> ObtenerAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Horario horario = await this.Cliente.PeticionGetAsync<Horario>($"Horarios/{id}");

            if (horario.Respuesta is null || horario.Respuesta.Mensaje.Equals(String.Empty)) return horario;

            this.Registro.LogError($"Error obteniendo horario: {horario.Respuesta.Mensaje}");
            throw new Exception(horario.Respuesta.Mostrar ? horario.Respuesta.Mensaje : "Error obteniendo horario");
        }

        public async Task CrearAsync(Horario modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPostAsync<Respuesta>("Horarios", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error creando horario: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error creando horario");
        }

        public async Task EditarAsync(Horario modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPutAsync<Respuesta>("Horarios", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error editando horario: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error editando horario");
        }

        public async Task BorrarAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionDeleteAsync<Respuesta>($"Horarios/{id}");

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error borrando horario: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error borrando horario");
        }
    }
}
