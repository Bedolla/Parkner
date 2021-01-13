namespace Parkner.Data.Entities
{
    public class Cajon : Base
    {
        public string Nombre { get; set; }
        public bool Disponible { get; set; }
        public string EstacionamientoId { get; set; }

        public virtual Estacionamiento Estacionamiento { get; set; }
    }
}
