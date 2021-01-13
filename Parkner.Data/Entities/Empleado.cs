using System;
using System.Collections.Generic;

namespace Parkner.Data.Entities
{
    public class Empleado : Base
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

        // Estacionamientos donde trabaja el Empleado
        public virtual ICollection<Estacionamiento> Estacionamientos { get; set; } = new HashSet<Estacionamiento>();

        // Reservas iniciadas por el empleado
        public virtual ICollection<Reserva> ReservasIniciadas { get; set; } = new HashSet<Reserva>();

        // Reservas finalizadas por el empleado
        public virtual ICollection<Reserva> ReservasFinalizadas { get; set; } = new HashSet<Reserva>();
    }
}
