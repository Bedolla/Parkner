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
    public class CajonesController : ControllerBase
    {
        public CajonesController
        (
            ILogger<CajonesController> registro,
            IRepositorioCajones cajones
        )
        {
            this.Registro = registro;
            this.Cajones = cajones;
        }

        private IRepositorioCajones Cajones { get; }

        private ILogger<CajonesController> Registro { get; }

        // GET: api/Cajones
        [HttpGet]
        public async Task<ActionResult<ListaPaginada<Cajon>>> Get([FromQuery] PaginacionPeticion? modelo)
        {
            try
            {
                return await this.Cajones.ObtenerTodoAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a los cajones en [GET] api/Cajones: {excepcion.Message}");
                return new ListaPaginada<Cajon> {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // GET: api/Cajones/De
        [Route("De")]
        [HttpGet]
        public async Task<ActionResult<ListaPaginada<Cajon>>> De([FromQuery] PaginacionPeticion? modelo)
        {
            try
            {
                return await this.Cajones.DeAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a los cajones en [GET] api/Reservas/De: {excepcion.Message}");
                return new ListaPaginada<Cajon> { Respuesta = new Respuesta { Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error } };
            }
        }

        // GET: api/Cajones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cajon>> Get(string id)
        {
            try
            {
                return await this.Cajones.ObtenerAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener un cajó en [GET] api/Cajones: {excepcion.Message}");
                return new Cajon {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // PUT: api/Cajones
        [HttpPut]
        public async Task<ActionResult<Respuesta>> Put(Cajon modelo)
        {
            try
            {
                return await this.Cajones.EditarAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al editar un cajó en [PUT] api/Cajones: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // POST: api/Cajones
        [HttpPost]
        public async Task<ActionResult<Respuesta>> Post(Cajon modelo)
        {
            try
            {
                return await this.Cajones.CrearAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al crear un cajón en [POST] api/Cajones: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // DELETE: api/Cajones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Respuesta>> Delete(string id)
        {
            try
            {
                return await this.Cajones.BorrarAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al borrar un cajón en [DELETE] api/Cajones: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }
    }
}
