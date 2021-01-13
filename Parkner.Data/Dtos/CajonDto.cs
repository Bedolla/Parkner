namespace Parkner.Data.Dtos
{
    public class CajonDto : BaseDto
    {
        public string Nombre { get; set; }
        public bool Disponible { get; set; }
        public string EstacionamientoId { get; set; }

        public EstacionamientoDto Estacionamiento { get; set; }
    }
}
