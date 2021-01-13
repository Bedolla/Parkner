using System;
using System.Collections.Generic;

namespace Parkner.Data.Dtos
{
    public class ListaPaginada<T> where T : class
    {
        public List<T> Lista { get; set; }
        public PaginacionMetaData MetaData { get; set; }
        public Respuesta Respuesta { get; set; }
    }

    public class PaginacionPeticion
    {
        public string Id { get; set; }
        public int NumeroPagina { get; set; } = 1;
        public int CantidadPorPagina { get; set; } = 100;
        public string TerminoBuscado { get; set; } = String.Empty;
        public string OrdenarPor { get; set; }
    }

    public class PaginacionMetaData
    {
        public int PaginaActual { get; set; }
        public int PaginasTotales { get; set; }
        public int TamanoPagina { get; set; }
        public int CantidadTotal { get; set; }

        public bool TieneAnterior => this.PaginaActual > 1;
        public bool TieneSiguiente => this.PaginaActual < this.PaginasTotales;
    }
}
