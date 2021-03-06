﻿namespace Parkner.Data.Entities
{
    public class Direccion : Base
    {
        public string Numero { get; set; }
        public string Calle { get; set; }
        public string EntreCalles { get; set; }
        public string Colonia { get; set; }
        public string CodigoPostal { get; set; }
        public string Municipio { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string EstacionamientoId { get; set; }

        public virtual Estacionamiento Estacionamiento { get; set; }
    }
}
