using System;

namespace Parkner.Data.Entities
{
    public class Ganancia : Base
    {
        public DateTime Fecha { get; set; }
        public decimal Cantidad { get; set; }
        public string ResponsableId { get; set; }

        public virtual Responsable Responsable { get; set; }
    }
}
