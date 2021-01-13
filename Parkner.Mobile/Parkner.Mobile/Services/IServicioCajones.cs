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
    public interface IServicioCajones
    {
        Task<ListaPaginada<Cajon>> ObtenerTodosAsync(PaginacionPeticion modelo);
        Task<ListaPaginada<Cajon>> ObtenerTodosAsync();
        Task<ListaPaginada<Cajon>> ObtenerDeAsync(string id);
        Task<Cajon> ObtenerAsync(string id);
        Task CrearAsync(Cajon modelo);
        Task EditarAsync(Cajon modelo);
        Task BorrarAsync(string id);
    }

    internal class ServicioCajones : IServicioCajones
    {
        public ServicioCajones(HttpClient cliente) => this.Cliente = cliente;

        private HttpClient Cliente { get; }

        public async Task<ListaPaginada<Cajon>> ObtenerTodosAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<ListaPaginada<Cajon>>("Cajones".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor},
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()}
            }));
        }

        public async Task<ListaPaginada<Cajon>> ObtenerTodosAsync()
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<ListaPaginada<Cajon>>("Cajones");
        }

        public async Task<ListaPaginada<Cajon>> ObtenerDeAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<ListaPaginada<Cajon>>("Cajones/De".AgregarCadenaConsulta(new Dictionary<string, string> {{"Id", id}}));
        }

        public async Task<Cajon> ObtenerAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<Cajon>($"Cajones/{id}");
        }

        public async Task CrearAsync(Cajon modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionPostAsync("Cajones", modelo);
        }

        public async Task EditarAsync(Cajon modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionPutAsync("Cajones", modelo);
        }

        public async Task BorrarAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionDeleteAsync($"Cajones/{id}");
        }
    }
}
