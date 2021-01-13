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
    public class ResponsablesController : ControllerBase
    {
        public ResponsablesController
        (
            ILogger<ResponsablesController> registro,
            IRepositorioResponsables responsables
        )
        {
            this.Registro = registro;
            this.Responsables = responsables;
        }

        private ILogger<ResponsablesController> Registro { get; }
        private IRepositorioResponsables Responsables { get; }

        // GET: api/Responsables
        [HttpGet]
        public async Task<ActionResult<ListaPaginada<Responsable>>> Get([FromQuery] PaginacionPeticion? modelo)
        {
            try
            {
                return await this.Responsables.ObtenerTodoAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a los responsables en [GET] api/Responsables: {excepcion.Message}");
                return new ListaPaginada<Responsable> {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // GET: api/Responsables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Responsable>> Get(string id)
        {
            try
            {
                return await this.Responsables.ObtenerAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a un responsable en [GET] api/Responsables: {excepcion.Message}");
                return new Responsable {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // PUT: api/Responsables
        [HttpPut]
        public async Task<ActionResult<Respuesta>> Put(Responsable modelo)
        {
            try
            {
                return await this.Responsables.EditarAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al editar un resposable en [PUT] api/Responsables: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // POST: api/Responsables
        [HttpPost]
        public async Task<ActionResult<Respuesta>> Post(Responsable modelo)
        {
            try
            {
                return await this.Responsables.CrearAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al crear un resposable en [POST] api/Responsables: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // DELETE: api/Responsables/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Respuesta>> Delete(string id)
        {
            try
            {
                return await this.Responsables.BorrarAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al borrar un resposable en [DELETE] api/Responsables: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }
    }
}
