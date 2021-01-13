using Microsoft.Extensions.Logging;
using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Parkner.Mobile.Services
{
    public interface IServicioSesion
    {
        Task CrearCliente(Cliente modelo);
        Task CrearResponsable(Responsable modelo);
        Task<Usuario> Ingresar(Sesion modelo);
        Task<Usuario> IngresarUsuario(Usuario modelo);
        Task<Cliente> IngresarCliente(Cliente modelo);
        Task<Responsable> IngresarResponsable(Responsable modelo);
    }

    public class ServicioSesion : IServicioSesion
    {
        public ServicioSesion
        (
            ILogger<ServicioSesion> registro,
            HttpClient cliente
        )
        {
            this.Registro = registro;
            this.Cliente = cliente;
        }

        private HttpClient Cliente { get; }
        private ILogger<ServicioSesion> Registro { get; }

        public async Task CrearCliente(Cliente modelo)
        {
            Respuesta respuesta = await this.Cliente.PeticionPostAsync<Respuesta>("Clientes", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error creando cliente: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error creando cliente");
        }

        public async Task CrearResponsable(Responsable modelo)
        {
            Respuesta respuesta = await this.Cliente.PeticionPostAsync<Respuesta>("Responsables", modelo);

            if (respuesta.Tipo.Equals(Tipos.Exito)) return;

            this.Registro.LogError($"Error creando cliente: {respuesta.Mensaje}");
            throw new Exception(respuesta.Mostrar ? respuesta.Mensaje : "Error creando cliente");
        }

        public async Task<Usuario> Ingresar(Sesion modelo)
        {
            Usuario usuario = await this.Cliente.PeticionPostAsync<Usuario>("Sesion/Ingresar", modelo);

            if (usuario.Respuesta is null || usuario.Respuesta.Mensaje.Equals(String.Empty)) return usuario;

            this.Registro.LogError($"Error validando usuario: {usuario.Respuesta.Mensaje}");
            throw new Exception(usuario.Respuesta.Mostrar ? usuario.Respuesta.Mensaje : "Credenciales no validas");
        }

        public async Task<Usuario> IngresarUsuario(Usuario modelo)
        {
            Usuario usuario = await this.Cliente.PeticionPostAsync<Usuario>("Sesion/IngresarUsuario", modelo);

            if (usuario.Respuesta is null || usuario.Respuesta.Mensaje.Equals(String.Empty)) return usuario;

            this.Registro.LogError($"Error validando usuario: {usuario.Respuesta.Mensaje}");
            throw new Exception(usuario.Respuesta.Mostrar ? usuario.Respuesta.Mensaje : "Credenciales no validas");
        }

        public async Task<Cliente> IngresarCliente(Cliente modelo)
        {
            Cliente cliente = await this.Cliente.PeticionPostAsync<Cliente>("Sesion/IngresarCliente", modelo);

            if (cliente.Respuesta is null || cliente.Respuesta.Mensaje.Equals(String.Empty)) return cliente;

            this.Registro.LogError($"Error validando usuario: {cliente.Respuesta.Mensaje}");
            throw new Exception(cliente.Respuesta.Mostrar ? cliente.Respuesta.Mensaje : "Credenciales no validas");
        }

        public async Task<Responsable> IngresarResponsable(Responsable modelo)
        {
            Responsable responsable = await this.Cliente.PeticionPostAsync<Responsable>("Sesion/IngresarResponsable", modelo);

            if (responsable.Respuesta is null || responsable.Respuesta.Mensaje.Equals(String.Empty)) return responsable;

            this.Registro.LogError($"Error validando usuario: {responsable.Respuesta.Mensaje}");
            throw new Exception(responsable.Respuesta.Mostrar ? responsable.Respuesta.Mensaje : "Credenciales no validas");
        }
    }
}
