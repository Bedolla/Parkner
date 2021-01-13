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
    public interface IServicioUsuarios
    {
        Task<ListaPaginada<Usuario>> ObtenerTodosAsync(PaginacionPeticion modelo);
        Task<Usuario> ObtenerAsync(string id);
        Task CrearAsync(Usuario modelo);
        Task EditarAsync(Usuario modelo);
        Task BorrarAsync(string id);
    }

    public class ServicioUsuarios : IServicioUsuarios
    {
        public ServicioUsuarios
        (
            HttpClient cliente,
            ILogger<ServicioUsuarios> registro,
            IConfiguration configuration
        )
        {
            this.Cliente = cliente;
            this.Registro = registro;
            this.Configuracion = configuration;
        }

        private IConfiguration Configuracion { get; }
        private HttpClient Cliente { get; }
        private ILogger<ServicioUsuarios> Registro { get; }

        public async Task<ListaPaginada<Usuario>> ObtenerTodosAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            ListaPaginada<Usuario> usuarios = await this.Cliente.PeticionGetAsync<ListaPaginada<Usuario>>("Usuarios".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()},
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor}
            }));

            if (usuarios.Respuesta is null || usuarios.Respuesta.Mensaje.Equals(String.Empty)) return usuarios;

            this.Registro.LogError($"Error obteniendo usuarios paginados: {usuarios.Respuesta.Mensaje}");
            throw new Exception(usuarios.Respuesta.Mostrar ? usuarios.Respuesta.Mensaje : "Error obteniendo usuarios");
        }

        public async Task<Usuario> ObtenerAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Usuario usuario = await this.Cliente.PeticionGetAsync<Usuario>($"Usuarios/{id}");

            if (usuario.Respuesta is null || usuario.Respuesta.Mensaje.Equals(String.Empty)) return usuario;

            this.Registro.LogError($"Error obteniendo usuario: {usuario.Respuesta.Mensaje}");
            throw new Exception(usuario.Respuesta.Mostrar ? usuario.Respuesta.Mensaje : "Error obteniendo usuario");
        }

        public async Task CrearAsync(Usuario modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPostAsync<Respuesta>("Usuarios", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error creando usuario: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error creando usuario");
        }

        public async Task EditarAsync(Usuario modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionPutAsync<Respuesta>("Usuarios", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error editando usuario: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error editando usuario");
        }

        public async Task BorrarAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Configuracion["Token"]);
            Respuesta respuesta = await this.Cliente.PeticionDeleteAsync<Respuesta>($"Usuarios/{id}");

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error borrando usuario: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error borrando usuario");
        }
    }
}
