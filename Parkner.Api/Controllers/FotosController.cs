using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Parkner.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Fotos")]
    public class FotosController : Controller
    {
        public FotosController(IWebHostEnvironment entorno) => this.Entorno = entorno ?? throw new ArgumentNullException(nameof(entorno));
        private IWebHostEnvironment Entorno { get; }

        // POST: api/Fotos/Empleados
        [Route("Empleados")]
        [HttpPost]
        public async Task Empleados(IFormFile foto) => await this.Guardar(foto, "empleados");

        // POST: api/Fotos/Responsables
        [Route("Responsables")]
        [HttpPost]
        public async Task Responsables(IFormFile foto) => await this.Guardar(foto, "responsables");

        // POST: api/Fotos/Clientes
        [Route("Clientes")]
        [HttpPost]
        public async Task Clientes(IFormFile foto) => await this.Guardar(foto, "clientes");

        // POST: api/Fotos/Usuarios
        [Route("Usuarios")]
        [HttpPost]
        public async Task Usuarios(IFormFile foto) => await this.Guardar(foto, "usuarios");

        // POST: api/Fotos/Estacionamientos
        [Route("Estacionamientos")]
        [HttpPost]
        public async Task Estacionamientos(IFormFile foto) => await this.Guardar(foto, "estacionamientos");

        private async Task Guardar(IFormFile foto, string tipo)
        {
            if (String.IsNullOrWhiteSpace(this.Entorno.WebRootPath)) this.Entorno.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            string fotos = Path.Combine(this.Entorno.WebRootPath, $"images/avatares/{tipo}");

            if (!Directory.Exists(fotos)) Directory.CreateDirectory(fotos);

            if (foto?.Length > 0)
            {
                await using FileStream fotoStream = new FileStream(Path.Combine(fotos, foto.FileName), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                await foto.CopyToAsync(fotoStream);
            }
        }

        // POST: api/Fotos/Borrar
        [Route("Borrar")]
        [HttpPost]
        public void Borrar(string foto)
        {
            if (String.IsNullOrWhiteSpace(this.Entorno.WebRootPath)) this.Entorno.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            string fotoPorBorrar = Path.Combine(this.Entorno.WebRootPath, foto);
            if (System.IO.File.Exists(fotoPorBorrar)) System.IO.File.Delete(fotoPorBorrar);
        }
    }
}
