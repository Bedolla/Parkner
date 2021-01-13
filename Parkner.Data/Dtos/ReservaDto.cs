using System;

namespace Parkner.Data.Dtos
{
    public class ReservaDto : BaseDto
    {
        public int Tolerancia { get; set; }
        public DateTime Llegada { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fin { get; set; }
        public decimal? Cobrado { get; set; }
        public bool Disponible { get; set; }
        public string ClienteId { get; set; }
        public string EstacionamientoId { get; set; }
        public string EmpleadoInicioId { get; set; }
        public string EmpleadoFinId { get; set; }

        public UsuarioDto Cliente { get; set; }
        public UsuarioDto EmpleadoInicio { get; set; }
        public UsuarioDto EmpleadoFin { get; set; }
        public EstacionamientoDto Estacionamiento { get; set; }
    }
}
