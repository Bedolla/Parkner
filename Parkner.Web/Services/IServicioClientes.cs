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
    public interface IServicioClientes
    {
        Task<ListaPaginada<Cliente>> ObtenerTodosAsync(PaginacionPeticion modelo);
        Task<Cliente> ObtenerAsync(string id);
        Task CrearAsync(Cliente modelo);
        Task EditarAsync(Cliente modelo);
        Task BorrarAsync(string id);
    }

    public class ServicioClientes : IServicioClientes
    {
        public ServicioClientes
        (
            HttpClient cliente,
            ILogger<ServicioClientes> registro,
            IConfiguration configuration
        )
        {
            this.Cliente = cliente;
            this.Registro = registro;
            this.Configuracion = configuration;
        }

        private IConfiguration Configuracion { get; }
        private HttpClient Cliente { get; }
        private ILogger<ServicioClientes> Registro { get; }

        public async Task<ListaPaginada<Cliente>> ObtenerTodosAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            ListaPaginada<Cliente> clientes = await this.Cliente.PeticionGetAsync<ListaPaginada<Cliente>>("Clientes".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()},
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor}
            }));

            if (clientes.Respuesta is null || clientes.Respuesta.Mensaje.Equals(String.Empty)) return clientes;

            this.Registro.LogError($"Error obteniendo clientes paginados: {clientes.Respuesta.Mensaje}");
            throw new Exception(clientes.Respuesta.Mostrar ? clientes.Respuesta.Mensaje : "Error obteniendo clientes");
        }

        public async Task<Cliente> ObtenerAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Cliente cliente = await this.Cliente.PeticionGetAsync<Cliente>($"Clientes/{id}");

            if (cliente.Respuesta is null || cliente.Respuesta.Mensaje.Equals(String.Empty)) return cliente;

            this.Registro.LogError($"Error obteniendo cliente: {cliente.Respuesta.Mensaje}");
            throw new Exception(cliente.Respuesta.Mostrar ? cliente.Respuesta.Mensaje : "Error obteniendo cliente");
        }

        public async Task CrearAsync(Cliente modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPostAsync<Respuesta>("Clientes", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error creando cliente: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error creando cliente");
        }

        public async Task EditarAsync(Cliente modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPutAsync<Respuesta>("Clientes", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error editando cliente: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error editando cliente");
        }

        public async Task BorrarAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionDeleteAsync<Respuesta>($"Clientes/{id}");

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error borrando cliente: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error borrando cliente");
        }
    }
}
