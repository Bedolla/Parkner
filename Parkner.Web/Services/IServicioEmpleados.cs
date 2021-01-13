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
    public interface IServicioEmpleados
    {
        Task<ListaPaginada<Empleado>> ObtenerTodosAsync(PaginacionPeticion modelo);
        Task<ListaPaginada<Empleado>> ObtenerDeAsync(PaginacionPeticion modelo);
        Task<List<Empleado>> ObtenerNoDeAsync(string id);
        Task<Empleado> ObtenerAsync(string id);
        Task CrearAsync(Empleado modelo);
        Task AgregarAlAsync(Empleado modelo);
        Task EditarAsync(Empleado modelo);
        Task BorrarAsync(string id);
        Task QuitarAsync(Empleado modelo);
    }

    public class ServicioEmpleados : IServicioEmpleados
    {
        public ServicioEmpleados
        (
            HttpClient cliente,
            ILogger<ServicioEmpleados> registro,
            IConfiguration configuration
        )
        {
            this.Cliente = cliente;
            this.Registro = registro;
            this.Configuracion = configuration;
        }

        private IConfiguration Configuracion { get; }
        private HttpClient Cliente { get; }
        private ILogger<ServicioEmpleados> Registro { get; }

        public async Task<ListaPaginada<Empleado>> ObtenerTodosAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            ListaPaginada<Empleado> empleados = await this.Cliente.PeticionGetAsync<ListaPaginada<Empleado>>("Empleados".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()},
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor}
            }));

            if (empleados.Respuesta is null || empleados.Respuesta.Mensaje.Equals(String.Empty)) return empleados;

            this.Registro.LogError($"Error obteniendo empleados paginados: {empleados.Respuesta.Mensaje}");
            throw new Exception(empleados.Respuesta.Mostrar ? empleados.Respuesta.Mensaje : "Error obteniendo empleados");
        }

        public async Task<ListaPaginada<Empleado>> ObtenerDeAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            ListaPaginada<Empleado> empleados = await this.Cliente.PeticionGetAsync<ListaPaginada<Empleado>>("Empleados/De".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"Id", modelo.Id},
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()},
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor}
            }));

            if (empleados.Respuesta is null || empleados.Respuesta.Mensaje.Equals(String.Empty)) return empleados;

            this.Registro.LogError($"Error obteniendo empleados paginados: {empleados.Respuesta.Mensaje}");
            throw new Exception(empleados.Respuesta.Mostrar ? empleados.Respuesta.Mensaje : "Error obteniendo empleados");
        }

        public async Task<List<Empleado>> ObtenerNoDeAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            List<Empleado> empleados = await this.Cliente.PeticionGetAsync<List<Empleado>>($"Empleados/NoDe/{id}");

            if (empleados is null || (empleados.Count <= 0)) return null;

            if (empleados[0].Respuesta is null || empleados[0].Respuesta.Mensaje.Equals(String.Empty)) return empleados;

            this.Registro.LogError($"Error obteniendo empleados paginados: {empleados[0].Respuesta.Mensaje}");
            throw new Exception(empleados[0].Respuesta.Mostrar ? empleados[0].Respuesta.Mensaje : "Error obteniendo empleados");
        }

        public async Task<Empleado> ObtenerAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Empleado empleado = await this.Cliente.PeticionGetAsync<Empleado>($"Empleados/{id}");

            if (empleado.Respuesta is null || empleado.Respuesta.Mensaje.Equals(String.Empty)) return empleado;

            this.Registro.LogError($"Error obteniendo empleado: {empleado.Respuesta.Mensaje}");
            throw new Exception(empleado.Respuesta.Mostrar ? empleado.Respuesta.Mensaje : "Error obteniendo empleado");
        }

        public async Task CrearAsync(Empleado modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPostAsync<Respuesta>("Empleados", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error creando empleado: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error creando empleado");
        }

        public async Task AgregarAlAsync(Empleado modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPostAsync<Respuesta>("Empleados/AgregarA", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error agregando empleado a estacionamiento: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error agregando empleado a estacionamiento");
        }

        public async Task EditarAsync(Empleado modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPutAsync<Respuesta>("Empleados", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error editando empleado: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error editando empleado");
        }

        public async Task BorrarAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionDeleteAsync<Respuesta>($"Empleados/{id}");

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error borrando empleado: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error borrando empleado");
        }

        public async Task QuitarAsync(Empleado modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPostAsync<Respuesta>("Empleados/QuitarDe", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error quitando empleado de estacionamiento: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error quitando empleado de estacionamiento");
        }
    }
}
