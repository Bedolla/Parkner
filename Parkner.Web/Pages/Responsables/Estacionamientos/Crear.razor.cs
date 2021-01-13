using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Parkner.Core.Constants;
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
    public partial class Crear
    {
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
        private EstacionamientoCrearModel Modelo { get; } = new EstacionamientoCrearModel();

        protected override async Task OnInitializedAsync() => await this.Api.ObtenerCredencialesAsync();

        private async void Enviar()
        {
            string id = Guid.NewGuid().ToString();
            string fotoNueva = $"images/avatares/estacionamientos/{id}.png";

            try
            {
                //await using (FileStream fotoArchivo = new FileStream($"wwwroot/{fotoNueva}", FileMode.Create, FileAccess.Write))
                //{
                //    this.FotoMemoria.WriteTo(fotoArchivo);
                //}

                await this.FotoMemoria.ToArray().SubirFotoAsync($"{id}.png", "Estacionamiento");

                await this.ServicioEstacionamientos.CrearAsync(new Estacionamiento
                {
                    Id = id,
                    Nombre = this.Modelo.Nombre,
                    Descripcion = this.Modelo.Descripcion,
                    Costo = this.Modelo.Costo,
                    Tipo = this.Modelo.Tipo,
                    Foto = fotoNueva,
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Concurrido = false,
                    ResponsableId = this.ResponsableState.Id,
                    Direccion = new Direccion
                    {
                        Id = id,
                        Numero = this.Modelo.Numero,
                        Calle = this.Modelo.Calle,
                        EntreCalles = this.Modelo.EntreCalles,
                        Colonia = this.Modelo.Colonia,
                        CodigoPostal = this.Modelo.CodigoPostal,
                        Municipio = this.Modelo.Municipio,
                        Latitud = this.Modelo.Latitud,
                        Longitud = this.Modelo.Longitud
                    }
                });

                this.Navegacion.NavigateTo($"/responsables/ver/{this.ResponsableState.Id}");
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);

                //if (File.Exists($"wwwroot/{fotoNueva}")) File.Delete($"wwwroot/{fotoNueva}");
                this.Modelo.Foto = String.Empty;
            }
            finally
            {
                if (this.FotoMemoria != null) await this.FotoMemoria.DisposeAsync();
            }
        }

        private void Cancelar() => this.Navegacion.NavigateTo($"/responsables/ver/{this.ResponsableState.Id}");

        private class EstacionamientoCrearModel
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
