using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parkner.Web.Shared.Tablas
{
    public partial class Desplegar
    {
        [Parameter]
        public List<DesplegarOpciones> Opciones { get; set; }

        [Parameter]
        public EventCallback<int> EnCantidadCambio { get; set; }

        private async Task CantidadCambio(ChangeEventArgs argumentos)
        {
            //if (argumentos.Value.ToString() == "-1") return;

            Int32.TryParse(argumentos.Value.ToString(), out int valor);

            await this.EnCantidadCambio.InvokeAsync(valor);
        }
    }

    public class DesplegarOpciones
    {
        public string Valor { get; set; }
        public string Texto { get; set; }
    }
}
