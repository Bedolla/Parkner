using Parkner.Core.Extensions;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using Parkner.Mobile.Helpers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Parkner.Mobile.Services
{
    public interface IServicioUsuarios
    {
        Task<List<Usuario>> ObtenerTodosAsync(PaginacionPeticion modelo);
        Task<List<Usuario>> ObtenerTodosAsync();
        Task<Usuario> ObtenerAsync(string id);
        Task CrearAsync(Usuario modelo);
        Task EditarAsync(Usuario modelo);
        Task BorrarAsync(string id);
    }

    internal class ServicioUsuarios : IServicioUsuarios
    {
        public ServicioUsuarios(HttpClient cliente) => this.Cliente = cliente;

        private HttpClient Cliente { get; }

        public async Task<List<Usuario>> ObtenerTodosAsync(PaginacionPeticion modelo) =>
            await this.Cliente.PeticionGetAsync<List<Usuario>>("Usuarios".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor},
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()}
            }));

        public async Task<List<Usuario>> ObtenerTodosAsync() => await this.Cliente.PeticionGetAsync<List<Usuario>>("Usuarios");

        public async Task<Usuario> ObtenerAsync(string id) => await this.Cliente.PeticionGetAsync<Usuario>($"Usuarios/{id}");

        public async Task CrearAsync(Usuario modelo) => await this.Cliente.PeticionPostAsync("Usuarios", modelo);

        public async Task EditarAsync(Usuario modelo) => await this.Cliente.PeticionPutAsync("Usuarios", modelo);

        public async Task BorrarAsync(string id) => await this.Cliente.PeticionDeleteAsync($"Usuarios/{id}");
    }
}
