namespace Parkner.Data.Dtos
{
    public class EstacionamientoEmpleadoDto : BaseDto
    {
        public string EstacionamientoId { get; set; }
        public string EmpleadoId { get; set; }

        public EstacionamientoDto Estacionamiento { get; set; }
        public UsuarioDto Empleado { get; set; }
    }
}
