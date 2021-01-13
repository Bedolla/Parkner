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
    public class ClientesController : ControllerBase
    {
        public ClientesController
        (
            ILogger<ClientesController> registro,
            IRepositorioClientes clientes
        )
        {
            this.Registro = registro;
            this.Clientes = clientes;
        }

        private IRepositorioClientes Clientes { get; }

        private ILogger<ClientesController> Registro { get; }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<ListaPaginada<Cliente>>> Get([FromQuery] PaginacionPeticion? paginacionPeticion)
        {
            try
            {
                return await this.Clientes.ObtenerTodoAsync(paginacionPeticion);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a los clientes en [GET] api/Clientes: {excepcion.Message}");
                return new ListaPaginada<Cliente> {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> Get(string id)
        {
            try
            {
                return await this.Clientes.ObtenerAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener un cliente en [GET] api/Clientes: {excepcion.Message}");
                return new Cliente {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // PUT: api/Clientes
        [HttpPut]
        public async Task<ActionResult<Respuesta>> Put(Cliente modelo)
        {
            try
            {
                return await this.Clientes.EditarAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al editar un cliente en [PUT] api/Clientes: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // POST: api/Clientes
        [HttpPost]
        public async Task<ActionResult<Respuesta>> Post(Cliente modelo)
        {
            try
            {
                return await this.Clientes.CrearAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al crear un cliente en [POST] api/Clientes: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Respuesta>> Delete(string id)
        {
            try
            {
                return await this.Clientes.BorrarAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al borrar un cliente en [DELETE] api/Clientes: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }
    }
}
