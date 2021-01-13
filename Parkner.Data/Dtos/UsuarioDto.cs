using System;
using System.Collections.Generic;

namespace Parkner.Data.Dtos
{
    public class UsuarioDto : BaseDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string Foto { get; set; }
        public DateTime Creacion { get; set; }
        public bool Disponible { get; set; }
        public string Tipo { get; set; }

        public List<Entities.Ganancia> Ganancias { get; set; }

        // Estacionamientos del Responsable
        public List<EstacionamientoDto> Estacionamientos { get; set; }

        // Reservas del Cliente
        public List<ReservaDto> Reservas { get; set; }

        // Estacionamientos del Empleado
        public List<EstacionamientoEmpleadoDto> EstacionamientosE { get; set; }

        // Reservas iniciadas por el empleado
        public List<ReservaDto> ReservasIniEmpleado { get; set; }

        // Reservas finalizadas por el empleado
        public List<ReservaDto> ReservasFinEmpleado { get; set; }
    }
}
