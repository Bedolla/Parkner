using Microsoft.AspNetCore.Components;
using System.Threading;

namespace Parkner.Web.Shared.Tablas
{
    public partial class Busqueda
    {
        private Timer Cronometro { get; set; }

        private string TerminoBuscado { get; set; }

        [Parameter]
        public EventCallback<string> EnBusquedaCambio { get; set; }

        private void BusquedaCambio()
        {
            this.Cronometro?.Dispose();
            this.Cronometro = new Timer(this.EnTiempoPaso, null, 500, 0);
        }

        private void EnTiempoPaso(object transmisor)
        {
            this.InvokeAsync(() => this.EnBusquedaCambio.InvokeAsync(this.TerminoBuscado));
            this.Cronometro.Dispose();
        }
    }
}
