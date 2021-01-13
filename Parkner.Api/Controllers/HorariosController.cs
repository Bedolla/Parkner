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
    public class HorariosController : ControllerBase
    {
        public HorariosController
        (
            ILogger<HorariosController> registro,
            IRepositorioHorarios horarios
        )
        {
            this.Registro = registro;
            this.Horarios = horarios;
        }

        private IRepositorioHorarios Horarios { get; }

        private ILogger<HorariosController> Registro { get; }

        // GET: api/Horarios
        [HttpGet]
        public async Task<ActionResult<ListaPaginada<Horario>>> Get([FromQuery] PaginacionPeticion? modelo)
        {
            try
            {
                return await this.Horarios.ObtenerTodoAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a los horarios en [GET] api/Horarios: {excepcion.Message}");
                return new ListaPaginada<Horario> {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // GET: api/Horarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Horario>> Get(string id)
        {
            try
            {
                return await this.Horarios.ObtenerAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener un horario en [GET] api/Horarios: {excepcion.Message}");
                return new Horario {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // PUT: api/Horarios
        [HttpPut]
        public async Task<ActionResult<Respuesta>> Put(Horario modelo)
        {
            try
            {
                return await this.Horarios.EditarAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al editar un horario en [PUT] api/Horarios: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // POST: api/Horarios
        [HttpPost]
        public async Task<ActionResult<Respuesta>> Post(Horario modelo)
        {
            try
            {
                return await this.Horarios.CrearAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al crear un horario en [POST] api/Horarios: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // DELETE: api/Horarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Respuesta>> Delete(string id)
        {
            try
            {
                return await this.Horarios.BorrarAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al borrar un horario en [DELETE] api/Horarios: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }
    }
}
