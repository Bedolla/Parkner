using System;

namespace Parkner.Data.Entities
{
    public class Usuario : Base
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string Rol { get; set; }
        public string Foto { get; set; }
        public DateTime Creacion { get; set; }
        public bool Disponible { get; set; }
        public string Token { get; set; }

        //// Ganancias del Responsable
        //public virtual ICollection<Ganancia> Ganancias { get; set; } = new HashSet<Ganancia>();

        //// Estacionamientos del Responsable
        //public virtual ICollection<Estacionamiento> Estacionamientos { get; set; } = new HashSet<Estacionamiento>();

        //// Reservas del Cliente
        //public virtual ICollection<Reserva> Reservas { get; set; } = new HashSet<Reserva>();

        //// Estacionamientos en los que trabaja el Empleado
        //public virtual ICollection<EstacionamientoEmpleado> EstacionamientosE { get; set; } = new HashSet<EstacionamientoEmpleado>();

        //// Reservas iniciadas por el empleado
        //public virtual ICollection<Reserva> ReservasIniEmpleado { get; set; } = new HashSet<Reserva>();

        //// Reservas finalizadas por el empleado
        //public virtual ICollection<Reserva> ReservasFinEmpleado { get; set; } = new HashSet<Reserva>();
    }
}
