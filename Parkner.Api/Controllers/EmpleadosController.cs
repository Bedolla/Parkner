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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parkner.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmpleadosController : ControllerBase
    {
        public EmpleadosController
        (
            ILogger<SesionController> registro,
            IRepositorioEmpleados empleados
        )
        {
            this.Registro = registro;
            this.Empleados = empleados;
        }

        private ILogger<SesionController> Registro { get; }
        private IRepositorioEmpleados Empleados { get; }

        // GET: api/Empleados
        [HttpGet]
        public async Task<ActionResult<ListaPaginada<Empleado>>> Get([FromQuery] PaginacionPeticion? paginacionPeticion)
        {
            try
            {
                return await this.Empleados.ObtenerTodoAsync(paginacionPeticion);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a los empleados en [GET] api/Empleados: {excepcion.Message}");
                return new ListaPaginada<Empleado> {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // GET: api/Empleados/De
        [Route("De")]
        [HttpGet]
        public async Task<ActionResult<ListaPaginada<Empleado>>> De([FromQuery] PaginacionPeticion? paginacionPeticion)
        {
            try
            {
                return await this.Empleados.DeAsync(paginacionPeticion);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a los empleados en [GET] api/Empleados/De: {excepcion.Message}");
                return new ListaPaginada<Empleado> {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // GET: api/Empleados/NoDe/5
        [Route("NoDe/{id}")]
        [HttpGet]
        public async Task<ActionResult<List<Empleado>>> NoDe(string id)
        {
            try
            {
                return await this.Empleados.NoDeAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a los empleados en [GET] api/Empleados/NoDe: {excepcion.Message}");
                return new List<Empleado> {new Empleado {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}}};
            }
        }

        // GET: api/Empleados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> Get(string id)
        {
            try
            {
                return await this.Empleados.ObtenerAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al obtener a un empleado en [GET] api/Empleados: {excepcion.Message}");
                return new Empleado {Respuesta = new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error}};
            }
        }

        // PUT: api/Empleados/5
        [HttpPut]
        public async Task<ActionResult<Respuesta>> Put(Empleado modelo)
        {
            try
            {
                return await this.Empleados.EditarAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al editar un empleado en [PUT] api/Empleados: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // POST: api/Empleados
        [HttpPost]
        public async Task<ActionResult<Respuesta>> Post(Empleado modelo)
        {
            try
            {
                return await this.Empleados.CrearAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al crear un empleado en [POST] api/Empleados: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // POST: api/Empleados/AgregarA
        [Route("AgregarA")]
        [HttpPost]
        public async Task<ActionResult<Respuesta>> AgregarA(Empleado modelo)
        {
            try
            {
                return await this.Empleados.AgregarAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al agregar un empleado a un estacionamiento en [POST] api/Empleados/AgregarA: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // POST: api/Empleados/QuitarDe
        [Route("QuitarDe")]
        [HttpPost]
        public async Task<ActionResult<Respuesta>> QuitarDe(Empleado modelo)
        {
            try
            {
                return await this.Empleados.QuitarAsync(modelo);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al quitar un empleado de un estacionamiento en [POST] api/Empleados/QuitarDe: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }

        // DELETE: api/Empleados/5
        [HttpDelete("{id}")]
        public async Task<Respuesta> Delete(string id)
        {
            try
            {
                return await this.Empleados.BorrarAsync(id);
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Se produjo un error al borrar un empleado en [DELETE] api/Empleados: {excepcion.Message}");
                return new Respuesta {Mensaje = excepcion.Message, Mostrar = true, Tipo = Tipos.Error};
            }
        }
    }
}
