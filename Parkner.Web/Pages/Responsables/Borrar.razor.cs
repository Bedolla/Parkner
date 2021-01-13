using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using System;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables
{
    [Authorize]
    public partial class Borrar
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioResponsables ServicioResponsables { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private ResponsableBorrarModel Modelo { get; } = new ResponsableBorrarModel();

        private Responsable Responsable { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            this.Responsable = await this.ServicioResponsables.ObtenerAsync(this.Id);
            this.Modelo.Nombre = this.Responsable.Nombre;
            this.Modelo.Apellido = this.Responsable.Apellido;
            this.Modelo.Correo = this.Responsable.Correo;
            this.Modelo.Foto = this.Responsable.Foto;
        }

        private async void Enviar(EditContext obj)
        {
            try
            {
                await this.ServicioResponsables.BorrarAsync(this.Id);
                await this.Responsable.Foto.BorrarFotoAsync();

                this.Navegacion.NavigateTo("/responsables");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo("/responsables");

        private class ResponsableBorrarModel
        {
            public string Nombre { get; set; }

            public string Apellido { get; set; }

            public string Correo { get; set; }

            public string Foto { get; set; }
        }
    }
}
