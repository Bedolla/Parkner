using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Clientes
{
    [Authorize]
    public partial class Ver
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioClientes ServicioClientes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private ClienteVerModel Modelo { get; } = new ClienteVerModel();

        private Cliente Cliente { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();
            
            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            this.Cliente = await this.ServicioClientes.ObtenerAsync(this.Id);
            this.Modelo.Nombre = this.Cliente.Nombre;
            this.Modelo.Apellido = this.Cliente.Apellido;
            this.Modelo.Correo = this.Cliente.Correo;
            this.Modelo.Foto = this.Cliente.Foto;
        }

        private void Cancelar() => this.Navegacion.NavigateTo("/clientes");

        private class ClienteVerModel
        {
            public string Nombre { get; set; }

            public string Apellido { get; set; }

            public string Correo { get; set; }

            public string Foto { get; set; }
        }
    }
}
