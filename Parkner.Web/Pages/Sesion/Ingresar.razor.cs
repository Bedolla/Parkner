using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.JSInterop;
using Parkner.Data.Entities;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Sesion
{
    public partial class Ingresar
    {
        [Inject]
        private IMensajes Mensajes { get; set; }

        [Inject]
        private AuthenticationStateProvider ProveedorEstadoAutenticacion { get; set; }

        [Inject]
        private ISessionStorageService ServicioAlmacenajeSesion { get; set; }

        [Inject]
        private NavigationManager Navegacion { get; set; }

        [Inject]
        private IServicioSesion ServicioSesion { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Inject]
        private IApi Api { get; set; }

        [Inject]
        private ILogger<Ingresar> Registro { get; set; }

        private IngresarModel Modelo { get; } = new IngresarModel();
        private Usuario Usuario { get; set; } = new Usuario();
        private string Regresar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this.Api.ObtenerCredencialesAsync();

            this.Recibir();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender) => await this.JsRuntime.InvokeVoidAsync("ColocarTemaIngresar");

        private void Recibir()
        {
            if (QueryHelpers.ParseQuery(this.Navegacion.ToAbsoluteUri(this.Navegacion.Uri).Query).TryGetValue("regresar", out StringValues regresarUrl)) this.Regresar = regresarUrl;
        }

        private async void Enviar()
        {
            try
            {
                this.Usuario.Correo = this.Modelo.Correo;
                this.Usuario.Clave = this.Modelo.Clave;

                this.Usuario = await this.ServicioSesion.IngresarUsuario(this.Usuario);

                ((ProveedorDeEstadoDeAutenticacion)this.ProveedorEstadoAutenticacion).IniciarSesion(this.Usuario);

                await this.ServicioAlmacenajeSesion.SetItemAsync("Id", this.Usuario.Id);
                await this.ServicioAlmacenajeSesion.SetItemAsync("Nombre", this.Usuario.Nombre);
                await this.ServicioAlmacenajeSesion.SetItemAsync("Apellido", this.Usuario.Apellido);
                await this.ServicioAlmacenajeSesion.SetItemAsync("Correo", this.Usuario.Correo);
                await this.ServicioAlmacenajeSesion.SetItemAsync("Rol", this.Usuario.Rol);
                await this.ServicioAlmacenajeSesion.SetItemAsync("Foto", this.Usuario.Foto);

                this.Navegacion.NavigateTo(this.Regresar ?? "/");
            }
            catch (Exception excepcion)
            {
                this.Registro.LogError($"Error al iniciar sesión en /ingresar: {excepcion.Message}");
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        private class IngresarModel
        {
            [Required(ErrorMessage = "Correo es obligatorio")]
            [EmailAddress(ErrorMessage = "Formato de correo incorrecto")]
            public string Correo { get; set; }

            [Required(ErrorMessage = "Contraseña es obligatoria")]
            public string Clave { get; set; }
        }
    }
}
