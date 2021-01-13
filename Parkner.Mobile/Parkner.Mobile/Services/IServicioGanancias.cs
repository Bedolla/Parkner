using Parkner.Core.Constants;
using Parkner.Core.Extensions;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;
using Parkner.Mobile.Helpers;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Parkner.Mobile.Services
{
    public interface IServicioGanancias
    {
        Task<ListaPaginada<Ganancia>> ObtenerTodosAsync(PaginacionPeticion modelo);
        Task<ListaPaginada<Ganancia>> ObtenerTodosAsync();
        Task<ListaPaginada<Ganancia>> ObtenerDeAsync(string id);
        Task<Ganancia> ObtenerAsync(string id);
        Task CrearAsync(Ganancia modelo);
        Task EditarAsync(Ganancia modelo);
        Task BorrarAsync(string id);
    }

    internal class ServicioGanancias : IServicioGanancias
    {
        public ServicioGanancias(HttpClient cliente) => this.Cliente = cliente;

        private HttpClient Cliente { get; }

        public async Task<ListaPaginada<Ganancia>> ObtenerTodosAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<ListaPaginada<Ganancia>>("Ganancias".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor},
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()}
            }));
        }

        public async Task<ListaPaginada<Ganancia>> ObtenerTodosAsync()
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<ListaPaginada<Ganancia>>("Ganancias");
        }

        public async Task<ListaPaginada<Ganancia>> ObtenerDeAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<ListaPaginada<Ganancia>>("Ganancias/De".AgregarCadenaConsulta(new Dictionary<string, string> {{"Id", id}}));
        }

        public async Task<Ganancia> ObtenerAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<Ganancia>($"Ganancias/{id}");
        }

        public async Task CrearAsync(Ganancia modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionPostAsync("Ganancias", modelo);
        }

        public async Task EditarAsync(Ganancia modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionPutAsync("Ganancias", modelo);
        }

        public async Task BorrarAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionDeleteAsync($"Ganancias/{id}");
        }
    }
}
