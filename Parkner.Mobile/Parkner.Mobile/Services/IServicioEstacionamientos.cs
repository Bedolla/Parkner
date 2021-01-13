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
    public interface IServicioEstacionamientos
    {
        Task<ListaPaginada<Estacionamiento>> ObtenerTodosAsync(PaginacionPeticion modelo);
        Task<ListaPaginada<Estacionamiento>> ObtenerTodosAsync();
        Task<ListaPaginada<Estacionamiento>> ObtenerDeAsync(string id);
        Task<Estacionamiento> ObtenerAsync(string id);
        Task CrearAsync(Estacionamiento modelo);
        Task EditarAsync(Estacionamiento modelo);
        Task BorrarAsync(string id);
    }

    internal class ServicioEstacionamientos : IServicioEstacionamientos
    {
        public ServicioEstacionamientos(HttpClient cliente) => this.Cliente = cliente;

        private HttpClient Cliente { get; }

        public async Task<ListaPaginada<Estacionamiento>> ObtenerTodosAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<ListaPaginada<Estacionamiento>>("Estacionamientos".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor},
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()}
            }));
        }

        public async Task<ListaPaginada<Estacionamiento>> ObtenerTodosAsync()
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<ListaPaginada<Estacionamiento>>("Estacionamientos");
        }

        public async Task<ListaPaginada<Estacionamiento>> ObtenerDeAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<ListaPaginada<Estacionamiento>>("Estacionamientos/De".AgregarCadenaConsulta(new Dictionary<string, string> { { "Id", id } }));
        }

        public async Task<Estacionamiento> ObtenerAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<Estacionamiento>($"Estacionamientos/{id}");
        }

        public async Task CrearAsync(Estacionamiento modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionPostAsync("Estacionamientos", modelo);
        }

        public async Task EditarAsync(Estacionamiento modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionPutAsync("Estacionamientos", modelo);
        }

        public async Task BorrarAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionDeleteAsync($"Estacionamientos/{id}");
        }
    }
}
