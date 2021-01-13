using System;
using System.Collections.Generic;

namespace Parkner.Data.Entities
{
    public class Estacionamiento : Base
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Costo { get; set; }
        public string Foto { get; set; }
        public decimal? Calificacion { get; set; }
        public string Tipo { get; set; }
        public DateTime Creacion { get; set; }
        public bool Disponible { get; set; }
        public bool Concurrido { get; set; }

        public string ResponsableId { get; set; }

        public virtual Direccion Direccion { get; set; }
        public virtual Responsable Responsable { get; set; }

        public virtual ICollection<Cajon> Cajones { get; set; } = new HashSet<Cajon>();
        public virtual ICollection<Reserva> Reservas { get; set; } = new HashSet<Reserva>();
        public virtual ICollection<Horario> Horarios { get; set; } = new HashSet<Horario>();
        public virtual ICollection<Empleado> Empleados { get; set; } = new HashSet<Empleado>();
    }
}
