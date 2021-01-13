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
    public class EstacionamientosController : ControllerBase
    {
        public EstacionamientosController
        (
            ILogger<EstacionamientosController> registro,
            IRepositorioEstacionamientos estacionamientos
        )
        {
            this.Registro = registro;
            this.Estacionamientos = estacionamientos;
        }

        private ILogger<EstacionamientosController> Registro { get; }
        private IRepositorioEstacionamientos Estacionamientos { get; }

        // GET: api/Estacionamientos
        [HttpGet]
        public async Task<ActionResult<ListaPaginada<Estacionamiento>>> Get([FromQuery] PaginacionPeticion? modelo)
        {
            try
            {
                return await this.Estacionamientos.ObtenerTodoAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a los estacionamientos en [GET] api/Estacionamientos: {excepcion.Message}");
                return new ListaPaginada<Estacionamiento> {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // GET: api/Estacionamientos/De
        [Route("De")]
        [HttpGet]
        public async Task<ActionResult<ListaPaginada<Estacionamiento>>> De([FromQuery] PaginacionPeticion? modelo)
        {
            try
            {
                return await this.Estacionamientos.DeAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a los estacionamientos en [GET] api/Estacionamientos/De: {excepcion.Message}");
                return new ListaPaginada<Estacionamiento> {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // GET: api/Estacionamientos/Guid
        [HttpGet("{id}")]
        public async Task<ActionResult<Estacionamiento>> Get(string id)
        {
            try
            {
                return await this.Estacionamientos.ObtenerAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a un estacionamiento en [GET] api/Estacionamientos: {excepcion.Message}");
                return new Estacionamiento {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // PUT: api/Estacionamientos
        [HttpPut]
        public async Task<ActionResult<Respuesta>> Put(Estacionamiento modelo)
        {
            try
            {
                return await this.Estacionamientos.EditarAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al editar un estacionamiento en [PUT] api/Estacionamientos: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // POST: api/Estacionamientos
        [HttpPost]
        public async Task<ActionResult<Respuesta>> Post(Estacionamiento modelo)
        {
            try
            {
                return await this.Estacionamientos.CrearAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al crear un estacionamiento en [POST] api/Estacionamientos: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // DELETE: api/Estacionamientos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Respuesta>> Delete(string id)
        {
            try
            {
                return await this.Estacionamientos.BorrarAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al borrar un estacionamiento en [DELETE] api/Estacionamientos: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }
    }
}
