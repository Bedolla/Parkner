using System;

namespace Parkner.Data.Entities
{
    public class Reserva : Base
    {
        public int Tolerancia { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public decimal? Cobrado { get; set; }
        public bool Disponible { get; set; }
        public string ClienteId { get; set; }
        public string EstacionamientoId { get; set; }
        public string EmpleadoInicializadorId { get; set; }
        public string EmpleadoFinalizadorId { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Empleado EmpleadoInicializador { get; set; }
        public virtual Empleado EmpleadoFinalizador { get; set; }
        public virtual Estacionamiento Estacionamiento { get; set; }
    }
}
