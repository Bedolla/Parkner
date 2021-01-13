using System;
using System.Collections.Generic;

namespace Parkner.Data.Entities
{
    public class Cliente : Base
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

        //public virtual ICollection<Reserva> Reservas { get; set; } = new HashSet<Reserva>();
        public List<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
