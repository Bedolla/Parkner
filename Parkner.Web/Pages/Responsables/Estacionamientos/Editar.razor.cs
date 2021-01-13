using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Core.Extensions;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using Parkner.Web.States;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Responsables.Estacionamientos
{
    [Authorize]
    public partial class Editar
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        private ResponsableState ResponsableState { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioEstacionamientos ServicioEstacionamientos { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private IApi Api { get; set; }

        private MemoryStream FotoMemoria { get; set; }
        private EstacionamientoEditarModel Modelo { get; } = new EstacionamientoEditarModel();
        private Estacionamiento Estacionamiento { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            await this.RecibirAsync();
        }

        private async Task RecibirAsync()
        {
            this.Estacionamiento = await this.ServicioEstacionamientos.ObtenerAsync(this.Id);
            this.Modelo.Nombre = this.Estacionamiento.Nombre;
            this.Modelo.Descripcion = this.Estacionamiento.Descripcion;
            this.Modelo.Costo = this.Estacionamiento.Costo;
            this.Modelo.Tipo = this.Estacionamiento.Tipo;
            this.Modelo.Foto = this.Estacionamiento.Foto;
            this.Modelo.Numero = this.Estacionamiento.Direccion.Numero;
            this.Modelo.Calle = this.Estacionamiento.Direccion.Calle;
            this.Modelo.EntreCalles = this.Estacionamiento.Direccion.EntreCalles;
            this.Modelo.Colonia = this.Estacionamiento.Direccion.Colonia;
            this.Modelo.CodigoPostal = this.Estacionamiento.Direccion.CodigoPostal;
            this.Modelo.Municipio = this.Estacionamiento.Direccion.Municipio;
            this.Modelo.Latitud = this.Estacionamiento.Direccion.Latitud;
            this.Modelo.Longitud = this.Estacionamiento.Direccion.Longitud;
        }

        private async void Enviar()
        {
            string fotoRuta = $"images/avatares/estacionamientos/{this.Estacionamiento.Id}.png";

            try
            {
                //string fotoVieja = this.Estacionamiento.Foto;

                if (this.FotoMemoria != null)
                {
                    //await using FileStream fotoArchivo = new FileStream($"wwwroot/{fotoRuta}", FileMode.Create, FileAccess.Write);
                    //this.FotoMemoria.WriteTo(fotoArchivo);

                    await this.FotoMemoria.ToArray().SubirFotoAsync($"{this.Estacionamiento.Id}.png", "Estacionamiento");
                    this.Estacionamiento.Foto = fotoRuta;
                }

                this.Estacionamiento.Nombre = this.Modelo.Nombre;
                this.Estacionamiento.Descripcion = this.Modelo.Descripcion;
                this.Estacionamiento.Costo = this.Modelo.Costo;
                this.Estacionamiento.Tipo = this.Modelo.Tipo;
                this.Estacionamiento.Direccion.Numero = this.Modelo.Numero;
                this.Estacionamiento.Direccion.Calle = this.Modelo.Calle;
                this.Estacionamiento.Direccion.EntreCalles = this.Modelo.EntreCalles;
                this.Estacionamiento.Direccion.Colonia = this.Modelo.Colonia;
                this.Estacionamiento.Direccion.CodigoPostal = this.Modelo.CodigoPostal;
                this.Estacionamiento.Direccion.Municipio = this.Modelo.Municipio;
                this.Estacionamiento.Direccion.Latitud = this.Modelo.Latitud;
                this.Estacionamiento.Direccion.Longitud = this.Modelo.Longitud;

                await this.ServicioEstacionamientos.EditarAsync(this.Estacionamiento);

                //if ((this.FotoMemoria != null) && File.Exists($"wwwroot/{fotoVieja}")) File.Delete($"wwwroot/{fotoVieja}");

                this.Navegacion.NavigateTo($"/responsables/ver/{this.ResponsableState.Id}");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);

                //if (File.Exists($"wwwroot/{fotoRuta}")) File.Delete($"wwwroot/{fotoRuta}");

                this.Modelo.Foto = String.Empty;
            }
            finally
            {
                if (this.FotoMemoria != null) await this.FotoMemoria.DisposeAsync();
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/ver/{this.ResponsableState.Id}");

        private class EstacionamientoEditarModel
        {
            [Required(ErrorMessage = "Nombre obligatorio")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "Descripción obligatoria")]
            public string Descripcion { get; set; }

            [Required(ErrorMessage = "Costo obligatorio")]
            public decimal Costo { get; set; }

            [Required(ErrorMessage = "Fotografía obligatoria")]
            public string Foto { get; set; }

            [Required(ErrorMessage = "Tipo obligatorio")]
            public string Tipo { get; set; }

            [Required(ErrorMessage = "Número obligatorio")]
            public string Numero { get; set; }

            [Required(ErrorMessage = "Calle obligatoria")]
            public string Calle { get; set; }

            [Required(ErrorMessage = "Entre calles obligatorio")]
            public string EntreCalles { get; set; }

            [Required(ErrorMessage = "Colonia obligatoria")]
            public string Colonia { get; set; }

            [Required(ErrorMessage = "Código postal obligatorio")]
            public string CodigoPostal { get; set; }

            [Required(ErrorMessage = "Municipio obligatorio")]
            public string Municipio { get; set; }

            [Required(ErrorMessage = "Latitud obligatoria")]
            public string Latitud { get; set; }

            [Required(ErrorMessage = "Longitud obligatoria")]
            public string Longitud { get; set; }
        }
    }
}
