using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Parkner.Api.Repositories;
using Parkner.Core.Constants;
using Parkner.Data;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using System;
using System.Threading.Tasks;

namespace Parkner.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SesionController : ControllerBase
    {
        public SesionController
        (
            ILogger<SesionController> registro,
            IRepositorioSesion sesion,
            IRepositorioUsuarios usuarios,
            IRepositorioClientes clientes,
            IRepositorioResponsables responsables,
            IRepositorioEmpleados empleados
        )
        {
            this.Registro = registro;
            this.Sesion = sesion;
            this.Usuarios = usuarios;
            this.Clientes = clientes;
            this.Responsables = responsables;
            this.Empleados = empleados;
        }

        private ILogger<SesionController> Registro { get; }
        private IRepositorioSesion Sesion { get; }
        private IRepositorioUsuarios Usuarios { get; }
        private IRepositorioClientes Clientes { get; }
        private IRepositorioResponsables Responsables { get; }
        private IRepositorioEmpleados Empleados { get; }

        // POST: api/Sesion
        // POST: api/Sesion/Autenticar
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> Autenticar(Usuario peticion)
        {
            try
            {
                return await this.Usuarios.AutenticarAsync(peticion);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al autenticar en [POST] api/Sesion/Ingresar: {excepcion.Message}");
                return new Usuario {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = false, Tipo = Tipos.Error}};
            }
        }

        // POST: api/Sesion/Ingresar
        [HttpPost]
        [Route("Ingresar")]
        public async Task<ActionResult<object>> Ingresar(Sesion peticion)
        {
            try
            {
                return await this.Sesion.AutenticarAsync(peticion);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al validar un usuario en [POST] api/Sesion/IngresarUsuario: {excepcion.Message}");
                return new Usuario { Respuesta = new Respuesta { Mensaje = excepcion.Message, Mostrar = false, Tipo = Tipos.Error } };
            }
        }

        // POST: api/Sesion/IngresarUsuario
        [HttpPost]
        [Route("IngresarUsuario")]
        public async Task<ActionResult<Usuario>> IngresarUsuario(Usuario peticion)
        {
            try
            {
                return await this.Usuarios.ValidarCredencialesAsync(peticion);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al validar un usuario en [POST] api/Sesion/IngresarUsuario: {excepcion.Message}");
                return new Usuario {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = false, Tipo = Tipos.Error}};
            }
        }

        // POST: api/Sesion/IngresarCliente
        [HttpPost]
        [Route("IngresarCliente")]
        public async Task<ActionResult<Cliente>> IngresarCliente(Cliente peticion)
        {
            try
            {
                return await this.Clientes.AutenticarAsync(peticion);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al autenticar un cliente en [POST] api/Sesion/IngresarCliente: {excepcion.Message}");
                return new Cliente {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = false, Tipo = Tipos.Error}};
            }
        }

        // POST: api/Sesion/IngresarResponsable
        [HttpPost]
        [Route("IngresarResponsable")]
        public async Task<ActionResult<Responsable>> IngresarResponsable(Responsable peticion)
        {
            try
            {
                return await this.Responsables.ValidarCredencialesAsync(peticion);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al validar un cliente en [POST] api/Sesion/IngresarResponsable: {excepcion.Message}");
                return new Responsable {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = false, Tipo = Tipos.Error}};
            }
        }

        // POST: api/Sesion/IngresarEmpleado
        [HttpPost]
        [Route("IngresarEmpleado")]
        public async Task<ActionResult<Empleado>> IngresarEmpleado(Empleado peticion)
        {
            try
            {
                return await this.Empleados.ValidarCredencialesAsync(peticion);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al validar un empleado en [POST] api/Sesion/IngresarEmpleado: {excepcion.Message}");
                return new Empleado {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = false, Tipo = Tipos.Error}};
            }
        }
    }
}
