using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables
{
    [Authorize]
    public partial class Editar
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

        private MemoryStream FotoMemoria { get; set; }
        private ResponsableEditarModel Modelo { get; } = new ResponsableEditarModel();

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
        }

        private async void Enviar()
        {
            string fotonueva = $"images/avatares/responsables/{this.Responsable.Id}.png";

            try
            {
                //string fotoVieja = this.Responsable.Foto;

                if (this.FotoMemoria != null)
                {
                    //await using FileStream fotoArchivo = new FileStream($"wwwroot/{fotonueva}", FileMode.Create, FileAccess.Write);
                    //this.FotoMemoria.WriteTo(fotoArchivo);

                    await this.FotoMemoria.ToArray().SubirFotoAsync($"{this.Responsable.Id}.png", this.Responsable.Rol);
                    this.Responsable.Foto = fotonueva;
                }

                this.Responsable.Nombre = this.Modelo.Nombre;
                this.Responsable.Apellido = this.Modelo.Apellido;
                this.Responsable.Correo = this.Modelo.Correo;
                this.Responsable.Clave = ((this.Modelo.Clave != null) && (this.Modelo.ConfirmacionClave != null)) ? this.Modelo.Clave.Encriptar() : this.Responsable.Clave;

                await this.ServicioResponsables.EditarAsync(this.Responsable);

                //if ((this.FotoMemoria != null) && File.Exists($"wwwroot/{fotoVieja}")) File.Delete($"wwwroot/{fotoVieja}");

                this.Navegacion.NavigateTo("/responsables");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);

                //if (File.Exists($"wwwroot/{fotonueva}")) File.Delete($"wwwroot/{fotonueva}");

                this.Modelo.Foto = String.Empty;
            }
            finally
            {
                if (this.FotoMemoria != null) await this.FotoMemoria.DisposeAsync();
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo("/responsables");

        private class ResponsableEditarModel
        {
            [Required(ErrorMessage = "Nombre(s) obligatorio(s)")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "Apellido(s) obligatorio(s)")]
            public string Apellido { get; set; }

            [Required(ErrorMessage = "Correo obligatorio")]
            [EmailAddress(ErrorMessage = "Formato de correo incorrecto")]
            public string Correo { get; set; }

            public string Clave { get; set; }

            [CompareProperty("Clave", ErrorMessage = "Debe ser igual a la contraseña")]
            public string ConfirmacionClave { get; set; }

            public string Foto { get; set; }
        }
    }
}
