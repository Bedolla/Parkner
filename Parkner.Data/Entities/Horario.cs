using System;

namespace Parkner.Data.Entities
{
    public class Horario : Base
    {
        public string Dia { get; set; }
        public int DiaNumero { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string EstacionamientoId { get; set; }

        public virtual Estacionamiento Estacionamiento { get; set; }
    }
}
