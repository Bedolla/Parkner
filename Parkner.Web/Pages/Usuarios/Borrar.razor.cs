using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using System;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Usuarios
{
    [Authorize]
    public partial class Borrar
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioUsuarios ServicioUsuarios { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private UsuarioBorrarModel Modelo { get; } = new UsuarioBorrarModel();

        private Usuario Usuario { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            this.Usuario = await this.ServicioUsuarios.ObtenerAsync(this.Id);
            this.Modelo.Nombre = this.Usuario.Nombre;
            this.Modelo.Apellido = this.Usuario.Apellido;
            this.Modelo.Correo = this.Usuario.Correo;
            this.Modelo.Foto = this.Usuario.Foto;
        }

        private async void Enviar(EditContext obj)
        {
            try
            {
                await this.ServicioUsuarios.BorrarAsync(this.Id);
                await this.Usuario.Foto.BorrarFotoAsync();

                this.Navegacion.NavigateTo("/usuarios");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo("/usuarios");

        private class UsuarioBorrarModel
        {
            public string Nombre { get; set; }

            public string Apellido { get; set; }

            public string Correo { get; set; }

            public string Foto { get; set; }
        }
    }
}
