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
    public class GananciasController : ControllerBase
    {
        public GananciasController
        (
            ILogger<GananciasController> registro,
            IRepositorioGanancias ganancias
        )
        {
            this.Registro = registro;
            this.Ganancias = ganancias;
        }

        private ILogger<GananciasController> Registro { get; }
        private IRepositorioGanancias Ganancias { get; }

        // GET: api/Ganancias
        [HttpGet]
        public async Task<ActionResult<ListaPaginada<Ganancia>>> Get([FromQuery] PaginacionPeticion? modelo)
        {
            try
            {
                return await this.Ganancias.ObtenerTodoAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener las ganancias en [GET] api/Ganancias: {excepcion.Message}");
                return new ListaPaginada<Ganancia> {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // GET: api/Ganancias/De
        [Route("De")]
        [HttpGet]
        public async Task<ActionResult<ListaPaginada<Ganancia>>> De([FromQuery] PaginacionPeticion? modelo)
        {
            try
            {
                return await this.Ganancias.DeAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener las ganancias en [GET] api/Ganancias/De: {excepcion.Message}");
                return new ListaPaginada<Ganancia> {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // GET: api/Ganancias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ganancia>> Get(string id)
        {
            try
            {
                return await this.Ganancias.ObtenerAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener una ganancia en [GET] api/Ganancias: {excepcion.Message}");
                return new Ganancia {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // PUT: api/Ganancias/
        [HttpPut]
        public async Task<ActionResult<Respuesta>> Put(Ganancia modelo)
        {
            try
            {
                return await this.Ganancias.EditarAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al editar una ganancia en [PUT] api/Ganancias: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // POST: api/Ganancias
        [HttpPost]
        public async Task<ActionResult<Respuesta>> Post(Ganancia modelo)
        {
            try
            {
                return await this.Ganancias.CrearAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al crear una ganancia en [POST] api/Ganancias: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // DELETE: api/Ganancias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Respuesta>> Delete(string id)
        {
            try
            {
                return await this.Ganancias.BorrarAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al borrar una ganancia en [DELETE] api/Ganancias: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }
    }
}
