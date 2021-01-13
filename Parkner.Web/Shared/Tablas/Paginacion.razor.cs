using Microsoft.AspNetCore.Components;
using Parkner.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parkner.Web.Shared.Tablas
{
    public partial class Paginacion
    {
        private List<PaginacionEnlace> Enlaces { get; set; }

        [Parameter]
        public PaginacionMetaData MetaData { get; set; }

        [Parameter]
        public int Separacion { get; set; }

        [Parameter]
        public EventCallback<int> PaginaSeleccionada { get; set; }

        protected override void OnParametersSet() => this.CrearEnlacesPaginacion();

        private void CrearEnlacesPaginacion()
        {
            this.Enlaces = new List<PaginacionEnlace> {new PaginacionEnlace(this.MetaData.PaginaActual - 1, this.MetaData.TieneAnterior, "«")};

            for (int i = 1; i <= this.MetaData.PaginasTotales; i++)
            {
                if ((i >= (this.MetaData.PaginaActual - this.Separacion)) && (i <= (this.MetaData.PaginaActual + this.Separacion))) this.Enlaces.Add(new PaginacionEnlace(i, true, i.ToString()) {Activo = this.MetaData.PaginaActual == i});
            }

            this.Enlaces.Add(new PaginacionEnlace(this.MetaData.PaginaActual + 1, this.MetaData.TieneSiguiente, "»"));
        }

        private async Task ActualizarPaginaActual(PaginacionEnlace paginacionEnlace)
        {
            if ((paginacionEnlace.Pagina == this.MetaData.PaginaActual) || !paginacionEnlace.Habilitado) return;

            this.MetaData.PaginaActual = paginacionEnlace.Pagina;
            await this.PaginaSeleccionada.InvokeAsync(paginacionEnlace.Pagina);
        }
    }

    public class PaginacionEnlace
    {
        public PaginacionEnlace(int pagina, bool habilitado, string texto)
        {
            this.Pagina = pagina;
            this.Habilitado = habilitado;
            this.Texto = texto;
        }

        public string Texto { get; set; }
        public int Pagina { get; set; }
        public bool Habilitado { get; set; }
        public bool Activo { get; set; }
    }
}
