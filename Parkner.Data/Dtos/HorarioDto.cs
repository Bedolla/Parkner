using System;

namespace Parkner.Data.Dtos
{
    public class HorarioDto : BaseDto
    {
        public string Dia { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string EstacionamientoId { get; set; }

        public EstacionamientoDto Estacionamiento { get; set; }
    }
}
