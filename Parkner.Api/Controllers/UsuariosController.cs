#nullable enable
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuariosController : ControllerBase
    {
        public UsuariosController
        (
            ILogger<SesionController> registro,
            IRepositorioUsuarios usuarios
        )
        {
            this.Registro = registro;
            this.Usuarios = usuarios;
        }

        private ILogger<SesionController> Registro { get; }
        private IRepositorioUsuarios Usuarios { get; }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<ListaPaginada<Usuario>>> Get([FromQuery] PaginacionPeticion? modelo)
        {
            try
            {
                return await this.Usuarios.ObtenerTodoAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a los usuarios en [GET] api/Usuarios: {excepcion.Message}");
                return new ListaPaginada<Usuario> {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> Get(string id)
        {
            try
            {
                return await this.Usuarios.ObtenerAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a un usuario en [GET] api/Usuarios: {excepcion.Message}");
                return new Usuario {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // PUT: api/Usuarios
        [HttpPut]
        public async Task<ActionResult<Respuesta>> Put(Usuario modelo)
        {
            try
            {
                return await this.Usuarios.EditarAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al editar un usuario en [PUT] api/Usuarios: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Respuesta>> Post(Usuario modelo)
        {
            try
            {
                return await this.Usuarios.CrearAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al crear un usuario en [POST] api/Usuarios: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Respuesta>> Delete(string id)
        {
            try
            {
                return await this.Usuarios.BorrarAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al borrar un usuario en [DELETE] api/Usuarios: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }
    }
}
