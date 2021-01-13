using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parkner.Web.Shared.Tablas
{
    public partial class Ordenar
    {
        [Parameter] public List<OrdenarOpciones> Opciones { get; set; }

        [Parameter] public EventCallback<string> EnOrdenCambio { get; set; }

        [Parameter] public string Seleccionado { get; set; }

        private async Task OrdenCambio(ChangeEventArgs argumentos)
        {
            //if (argumentos.Value.ToString() == "-1") return;

            await this.EnOrdenCambio.InvokeAsync(argumentos.Value?.ToString());
        }
    }

    public class OrdenarOpciones
    {
        public string Valor { get; set; }
        public string Texto { get; set; }
    }
}