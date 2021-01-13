using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.States;
using System;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables
{
    [Authorize]
    public partial class Ver
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        private ResponsableState ResponsableState { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioResponsables ServicioResponsables { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private ResponsableVerModel Modelo { get; } = new ResponsableVerModel();

        private Responsable Responsable { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            this.ResponsableState.Id = this.Id;

            this.Responsable = await this.ServicioResponsables.ObtenerAsync(this.Id);
            this.Modelo.Nombre = this.Responsable.Nombre;
            this.Modelo.Apellido = this.Responsable.Apellido;
            this.Modelo.Correo = this.Responsable.Correo;
            this.Modelo.Foto = this.Responsable.Foto;
            this.Modelo.Creacion = this.Responsable.Creacion;
        }

        private void Cancelar() => this.Navegacion.NavigateTo("/responsables");

        private class ResponsableVerModel
        {
            public string Nombre { get; set; }

            public string Apellido { get; set; }

            public string Correo { get; set; }

            public DateTime Creacion { get; set; }

            public string Foto { get; set; }
        }
    }
}
