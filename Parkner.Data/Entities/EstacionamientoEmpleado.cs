using System;

namespace Parkner.Data.Entities
{
    public class EstacionamientoEmpleado : Base
    {
        public string EstacionamientoId { get; set; }
        public string EmpleadoId { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Estacionamiento Estacionamiento { get; set; }
        public virtual Empleado Empleado { get; set; }
    }
}
