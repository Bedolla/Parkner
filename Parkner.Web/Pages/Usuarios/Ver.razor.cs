using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Usuarios
{
    [Authorize]
    public partial class Ver
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioUsuarios ServicioUsuarios { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private UsuarioVerModel Modelo { get; } = new UsuarioVerModel();

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

        private void Cancelar() => this.Navegacion.NavigateTo("/usuarios");

        private class UsuarioVerModel
        {
            public string Nombre { get; set; }

            public string Apellido { get; set; }

            public string Correo { get; set; }

            public string Foto { get; set; }
        }
    }
}
