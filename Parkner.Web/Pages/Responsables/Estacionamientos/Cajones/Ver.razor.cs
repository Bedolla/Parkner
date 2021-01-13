using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.States;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Estacionamientos.Cajones
{
    [Authorize]
    public partial class Ver
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        private EstacionamientoState EstacionamientoState { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioCajones ServicioCajones { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private CajonVerModel Modelo { get; } = new CajonVerModel();

        private Cajon Cajon { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            this.Cajon = await this.ServicioCajones.ObtenerAsync(this.Id);
            this.Modelo.Nombre = this.Cajon.Nombre;
            this.Modelo.Disponible = this.Cajon.Disponible;
        }

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/estacionamientos/ver/{this.EstacionamientoState.Id}");

        private class CajonVerModel
        {
            public string Nombre { get; set; }

            public bool Disponible { get; set; }
        }
    }
}
