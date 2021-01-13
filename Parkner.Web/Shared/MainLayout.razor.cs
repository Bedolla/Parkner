using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Parkner.Web.Helpers.Pages;
using Parkner.Web.States;
using System;
using System.Threading.Tasks;

namespace Parkner.Web.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private ISessionStorageService ServicioAlmacenajeSesion { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Inject]
        private IApi Api { get; set; }

        [Inject]
        private UsuarioState UsuarioState { get; set; }

        [Inject]
        private IMensajes Mensajes { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await this.Api.ObtenerCredencialesAsync();

                this.UsuarioState.Id = await this.ServicioAlmacenajeSesion.GetItemAsync<string>("Id");
                this.UsuarioState.Nombre = await this.ServicioAlmacenajeSesion.GetItemAsync<string>("Nombre");
                this.UsuarioState.Apellido = await this.ServicioAlmacenajeSesion.GetItemAsync<string>("Apellido");
                this.UsuarioState.Correo = await this.ServicioAlmacenajeSesion.GetItemAsync<string>("Correo");
                this.UsuarioState.Rol = await this.ServicioAlmacenajeSesion.GetItemAsync<string>("Rol");
                this.UsuarioState.Foto = await this.ServicioAlmacenajeSesion.GetItemAsync<string>("Foto");

                this.UsuarioState.CambioUsuario();
            }
            catch (Exception excepcion)
            {
                this.Mensajes.MostrarError(excepcion.Message);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender) => await this.JsRuntime.InvokeVoidAsync("ColocarTemaGeneral");
    }
}
