using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Parkner.Web.Services;
using System.Threading.Tasks;

namespace Parkner.Web.Pages.Sesion
{
    [Authorize]
    public partial class Salir
    {
        [Inject]
        private AuthenticationStateProvider ProveedorEstadoAutenticacion { get; set; }

        [Inject]
        private NavigationManager AdministradorNavegacion { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ((ProveedorDeEstadoDeAutenticacion)this.ProveedorEstadoAutenticacion).CerrarSesion();
            this.AdministradorNavegacion.NavigateTo("/ingresar", true);
        }
    }
}
