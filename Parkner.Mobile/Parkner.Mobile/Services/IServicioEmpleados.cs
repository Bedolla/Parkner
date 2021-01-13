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
    public interface IServicioEmpleados
    {
        Task<List<Empleado>> ObtenerTodosAsync(PaginacionPeticion modelo);
        Task<List<Empleado>> ObtenerTodosAsync();
        Task<Empleado> ObtenerAsync(string id);
        Task CrearAsync(Empleado modelo);
        Task EditarAsync(Empleado modelo);
        Task BorrarAsync(string id);
        Task AgregarAlAsync(Empleado modelo);
        Task QuitarDeAsync(Empleado modelo);
    }

    internal class ServicioEmpleados : IServicioEmpleados
    {
        public ServicioEmpleados(HttpClient cliente) => this.Cliente = cliente;

        private HttpClient Cliente { get; }

        public async Task<List<Empleado>> ObtenerTodosAsync(PaginacionPeticion modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<List<Empleado>>("Empleados".AgregarCadenaConsulta(new Dictionary<string, string>
            {
                {"TerminoBuscado", modelo.TerminoBuscado},
                {"OrdenarPor", modelo.OrdenarPor},
                {"NumeroPagina", modelo.NumeroPagina.ToString()},
                {"CantidadPorPagina", modelo.CantidadPorPagina.ToString()}
            }));
        }

        public async Task<List<Empleado>> ObtenerTodosAsync()
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<List<Empleado>>("Empleados");
        }

        public async Task<Empleado> ObtenerAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            return await this.Cliente.PeticionGetAsync<Empleado>($"Empleados/{id}");
        }

        public async Task CrearAsync(Empleado modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionPostAsync("Empleados", modelo);
        }

        public async Task EditarAsync(Empleado modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionPutAsync("Empleados", modelo);
        }

        public async Task BorrarAsync(string id)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionDeleteAsync($"Empleados/{id}");
        }

        public async Task AgregarAlAsync(Empleado modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionPostAsync("Empleados/AgregarA", modelo);
        }

        public async Task QuitarDeAsync(Empleado modelo)
        {
            this.Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties[Propiedades.Token].ToString());
            await this.Cliente.PeticionPostAsync("Empleados/QuitarDe", modelo);
        }
    }
}
