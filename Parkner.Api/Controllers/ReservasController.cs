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
    public class ReservasController : ControllerBase
    {
        public ReservasController
        (
            ILogger<ReservasController> registro,
            IRepositorioReservas reservas
        )
        {
            this.Registro = registro;
            this.Reservas = reservas;
        }

        private IRepositorioReservas Reservas { get; }

        private ILogger<ReservasController> Registro { get; }

        // GET: api/Reservas
        [HttpGet]
        public async Task<ActionResult<ListaPaginada<Reserva>>> Get([FromQuery]PaginacionPeticion? modelo)
        {
            try
            {
                return await this.Reservas.ObtenerTodoAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a las reservas en [GET] api/Reservas: {excepcion.Message}");
                return new ListaPaginada<Reserva> {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // GET: api/Reservas/De
        [Route("De")]
        [HttpGet]
        public async Task<ActionResult<ListaPaginada<Reserva>>> De([FromQuery] PaginacionPeticion? modelo)
        {
            try
            {
                return await this.Reservas.DeAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a las reservas en [GET] api/Reservas/De: {excepcion.Message}");
                return new ListaPaginada<Reserva> {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // GET: api/Reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> Get(string id)
        {
            try
            {
                return await this.Reservas.ObtenerAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener una reserva en [GET] api/Reservas: {excepcion.Message}");
                return new Reserva {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // PUT: api/Reservas
        [HttpPut]
        public async Task<ActionResult<Respuesta>> Put(Reserva modelo)
        {
            try
            {
                return await this.Reservas.EditarAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al editar una reserva en [PUT] api/Reservas: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // POST: api/Reservas
        [HttpPost]
        public async Task<ActionResult<Respuesta>> Post(Reserva modelo)
        {
            try
            {
                return await this.Reservas.CrearAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al crear una reserva en [POST] api/Reservas: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // DELETE: api/Reservas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Respuesta>> Delete(string id)
        {
            try
            {
                return await this.Reservas.BorrarAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al borrar una reserva en [DELETE] api/Reservas: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }
    }
}
