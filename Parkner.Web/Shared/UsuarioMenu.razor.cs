using Microsoft.AspNetCore.Components;
using Parkner.Web.States;
using System;

namespace Parkner.Web.Shared
{
    public partial class UsuarioMenu : IDisposable
    {
        [Inject]
        private UsuarioState UsuarioState { get; set; }

        public void Dispose()
        {
            this.UsuarioState.EnCambioUsuario -= this.StateHasChanged;
            GC.SuppressFinalize(this);
        }

        protected override void OnInitialized()
        {
            this.UsuarioState.EnCambioUsuario += this.StateHasChanged;
        }
    }
}
