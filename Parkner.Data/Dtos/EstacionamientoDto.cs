using System;
using System.Collections.Generic;

namespace Parkner.Data.Dtos
{
    public class EstacionamientoDto : BaseDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Costo { get; set; }
        public string Foto { get; set; }
        public decimal Calificacion { get; set; }
        public string Tipo { get; set; }
        public DateTime Creacion { get; set; }
        public bool Disponible { get; set; }
        public bool Concurrido { get; set; }
        public string DireccionId { get; set; }
        public string ResponsableId { get; set; }

        public DireccionDto Direccion { get; set; }
        public UsuarioDto Responsable { get; set; }

        public List<CajonDto> Cajones { get; set; }
        public List<ReservaDto> Reservas { get; set; }
        public List<HorarioDto> Horarios { get; set; }
        public List<EstacionamientoEmpleadoDto> Empleados { get; set; }
    }
}
