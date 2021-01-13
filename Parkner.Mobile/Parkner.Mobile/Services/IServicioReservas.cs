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
    public interface IServicioReservas
    {
        Task<ListaPaginada<Reserva>> ObtenerTodosAsync(PaginacionPeticion modelo);
        Task<ListaPaginada<Reserva>> ObtenerTodosAsync();
        Task<ListaPaginada<Reserva>> ObtenerDeAsync(string id);
        Task<Reserva> ObtenerAsync(string id);
        Task CrearAsync(Reserva modelo);
        Task EditarAsync(Reserva modelo);
        Task BorrarAsync(string id);
    }

    internal class ServicioReservas : IServicioReservas
    {
        public ServicioReservas(HttpClient cliente) => this.Cliente = cliente;

        private HttpClient Cliente { get; }

        public async Task<ListaPaginada<Reserva>> ObtenerTodosAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<ListaPaginada<Reserva>>("Reservas".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor},
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()}
            }));
        }

        public async Task<ListaPaginada<Reserva>> ObtenerDeAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<ListaPaginada<Reserva>>("Reservas/De".AgregarCadenaConsulta(new Dictionary<string, string> {{"Id", id}}));
        }

        public async Task<ListaPaginada<Reserva>> ObtenerTodosAsync()
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<ListaPaginada<Reserva>>("Reservas");
        }

        public async Task<Reserva> ObtenerAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<Reserva>($"Reservas/{id}");
        }

        public async Task CrearAsync(Reserva modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionPostAsync("Reservas", modelo);
        }

        public async Task EditarAsync(Reserva modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionPutAsync("Reservas", modelo);
        }

        public async Task BorrarAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionDeleteAsync($"Reservas/{id}");
        }
    }
}
